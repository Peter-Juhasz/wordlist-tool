namespace WordlistTool.Core.Transforms.Library;

public sealed class FilterWhitespaceTransform : StreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		if (input.IsWhiteSpace())
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}

public sealed class AsciiFilterWhitespaceTransform : AsciiStreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		if (input.IndexOfAnyExcept((byte)0x09, (byte)0x0A, (byte)0x0D) == -1 &&
			input.IndexOfAnyExcept((byte)0x0B, (byte)0x0C, (byte)0x20) == -1
		)
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}