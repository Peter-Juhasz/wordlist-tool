using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Transforms;

public abstract class StreamingSpanTransform : ITransform<InputOptions, OutputOptions>
{
	private const int MaxStackSize = 256;

	protected virtual int EstimateOutputBufferSize(int inputLength) => inputLength;

	protected abstract void Transform(ReadOnlySpan<char> input, Span<char> output, out int written);

	public async Task ExecuteAsync(InputOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		PipeReader reader = input.GetPipeReader();
		PipeWriter writer = output.GetPipeWriter();

		while (true)
		{
			// try read
			var result = await reader.ReadAsync(cancellationToken);
			if (result.IsCanceled)
			{
				break;
			}

			// try find individual lines
			ReadOnlySequence<byte> buffer = result.Buffer;

			while (TryReadLine(ref buffer, input.LineEndingBytes, out ReadOnlySequence<byte> line))
			{
				ProcessLine(output, line, writer);
			}

			// flush
			if (writer.UnflushedBytes >= output.BufferSize)
			{
				await writer.FlushAsync(cancellationToken);
			}

			// advance reader
			reader.AdvanceTo(buffer.Start, buffer.End);

			// check whether we are at the end of the stream
			if (result.IsCompleted)
			{
				await writer.FlushAsync(cancellationToken);
				await writer.CompleteAsync();
				break;
			}
		}
	}

	private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, byte[] lineEnding, out ReadOnlySequence<byte> line)
	{
		// try to find next line separator
		var reader = new SequenceReader<byte>(buffer);
		if (reader.TryReadTo(out line, lineEnding, advancePastDelimiter: false))
		{
			buffer = buffer.Slice(line.Length + lineEnding.Length);
			return true;
		}

		line = default;
		return false;
	}

	private bool ProcessLine(OutputOptions outputOptions, ReadOnlySequence<byte> bytes, PipeWriter writer)
	{
		var lineEnding = outputOptions.LineEndingBytes;
		var encoding = outputOptions.Encoding;

		int inputLength = (int)bytes.Length;
		Span<char> inputBuffer = inputLength > MaxStackSize ? new char[inputLength] : stackalloc char[inputLength];
		int written = encoding.GetChars(bytes, inputBuffer);

		int estimatedOutputLength = EstimateOutputBufferSize(inputLength);
		Span<char> outputBuffer = estimatedOutputLength > MaxStackSize ? new char[estimatedOutputLength] : stackalloc char[estimatedOutputLength];
		Transform(inputBuffer[..written], outputBuffer, out written);

		if (written > 0)
		{
			var output = writer.GetSpan(encoding.GetMaxByteCount(written + lineEnding.Length));
			written = encoding.GetBytes(outputBuffer[..written], output);
			lineEnding.CopyTo(output[written..]);
			writer.Advance(written + lineEnding.Length);
			return true;
		}

		return false;
	}
}
