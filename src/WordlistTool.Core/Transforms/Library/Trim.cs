namespace WordlistTool.Core.Transforms.Library;

public sealed class TrimTransform : StreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		input.CopyTo(output);
		var trimmed = output[..input.Length].Trim();
		written = trimmed.Length;
	}
}