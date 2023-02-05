namespace WordlistTool.Core.Transforms.Library;

public sealed class ToLowerTransform : StreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		written = input.ToLowerInvariant(output);
	}
}

public sealed class AsciiToLowerTransform : AsciiStreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		// TODO: use new Ascii API in .NET 8
		//Ascii.ToLower(input, output, out written);
		throw new NotSupportedException();
	}
}


public sealed class ToUpperTransform : StreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		written = input.ToUpperInvariant(output);
	}
}

public sealed class AsciiToUpperTransform : AsciiStreamingSpanTransform
{
	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		// TODO: use new Ascii API in .NET 8
		//Ascii.ToUpper(input, output, out written);
		throw new NotSupportedException();
	}
}
