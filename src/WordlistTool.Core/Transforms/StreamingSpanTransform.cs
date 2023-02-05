using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Transforms;

public abstract class StreamingSpanTransform : ITransform<InputOptions, OutputOptions>
{
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

			while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
			{
				ProcessLine(output, line, writer);
			}

			// flush
			if (writer.UnflushedBytes > output.BufferSize)
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

	private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
	{
		// try to find next line separator
		SequencePosition? position = buffer.PositionOf((byte)'\n');

		// not found
		if (position == null)
		{
			line = default;
			return false;
		}

		// create a window in the byte sequence
		line = buffer.Slice(0, position.Value);
		buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
		return true;
	}

	private bool ProcessLine(OutputOptions outputOptions, ReadOnlySequence<byte> bytes, PipeWriter writer)
	{
		var lineEnding = outputOptions.LineEndingBytes;
		var encoding = outputOptions.Encoding;

		Span<char> inputBuffer = stackalloc char[(int)bytes.Length];
		int written = encoding.GetChars(bytes, inputBuffer);

		Span<char> outputBuffer = stackalloc char[EstimateOutputBufferSize(inputBuffer.Length)];
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
