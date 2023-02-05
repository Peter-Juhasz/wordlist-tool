namespace WordlistTool.Core.Transforms.Library;

public sealed class FilterMinimumLengthTransform : StreamingSpanTransform
{
	public FilterMinimumLengthTransform(int length)
	{
		Length = length;
	}

	public int Length { get; }

	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		if (input.Length < Length)
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}

public sealed class AsciiFilterMinimumLengthTransform : AsciiStreamingSpanTransform
{
	public AsciiFilterMinimumLengthTransform(int length)
	{
		Length = length;
	}

	public int Length { get; }

	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		if (input.Length < Length)
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}


public sealed class FilterMaximumLengthTransform : StreamingSpanTransform
{
	public FilterMaximumLengthTransform(int length)
	{
		Length = length;
	}

	public int Length { get; }

	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		if (input.Length > Length)
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}

public sealed class AsciiFilterMaximumLengthTransform : AsciiStreamingSpanTransform
{
	public AsciiFilterMaximumLengthTransform(int length)
	{
		Length = length;
	}

	public int Length { get; }

	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		if (input.Length > Length)
		{
			written = 0;
			return;
		}

		input.CopyTo(output);
		written = input.Length;
	}
}