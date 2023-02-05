namespace WordlistTool.Core.Transforms.Library;

public sealed class ReverseTransform : StreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		input.CopyTo(output);
		output[..input.Length].Reverse();
		written = input.Length;
	}
}

public sealed class AsciiReverseTransform : AsciiStreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		input.CopyTo(output);
		output[..input.Length].Reverse();
		written = input.Length;
	}
}