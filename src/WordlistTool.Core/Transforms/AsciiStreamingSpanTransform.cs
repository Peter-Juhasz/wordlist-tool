using System.Buffers;
using System.IO.Pipelines;

namespace WordlistTool.Core.Transforms;

public abstract class AsciiStreamingSpanTransform : ITransform<InputOptions, OutputOptions>
{
	protected virtual int EstimateOutputBufferSize(int inputLength) => inputLength;

	protected abstract void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written);

	public async Task ExecuteAsync(InputOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		PipeReader reader = input.GetPipeReader();
		PipeWriter writer = output.GetPipeWriter();
		var lineEnding = output.LineEndingBytes;

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
				ProcessLine(line, writer, lineEnding);
			}

			// advance reader
			reader.AdvanceTo(buffer.Start, buffer.End);

			// flush
			if (writer.UnflushedBytes > output.BufferSize)
			{
				await writer.FlushAsync(cancellationToken);
			}

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

	private bool ProcessLine(ReadOnlySequence<byte> bytes, PipeWriter writer, byte[] lineEnding)
	{
		var length = (int)bytes.Length;
		int written;

		if (bytes.IsSingleSegment)
		{
			var output = writer.GetSpan(sizeHint: EstimateOutputBufferSize(length) + lineEnding.Length);
			Transform(bytes.FirstSpan, output, out written);
			if (written > 0)
			{
				lineEnding.CopyTo(output[length..]);
				writer.Advance(written + lineEnding.Length);
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			Span<byte> buffer = stackalloc byte[EstimateOutputBufferSize(length)];
			bytes.CopyTo(buffer);
			var output = writer.GetSpan(sizeHint: length + lineEnding.Length);
			Transform(buffer, output, out written);
			if (written > 0)
			{
				lineEnding.CopyTo(output[length..]);
				writer.Advance(written + lineEnding.Length);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
