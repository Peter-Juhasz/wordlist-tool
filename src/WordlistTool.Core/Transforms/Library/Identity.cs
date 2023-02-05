namespace WordlistTool.Core.Transforms.Library;

public sealed class IdentityTransform : AsciiStreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		input.CopyTo(output);
		written = input.Length;
	}
}