using System.Text;

namespace WordlistTool.Core.Transforms.Library;

public sealed class PrependTransform : StreamingSpanTransform
{
	public PrependTransform(char[] value)
	{
		Value = value;
	}

	public char[] Value { get; }

	protected override int EstimateOutputBufferSize(int inputLength) => inputLength + Value.Length;

	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		Value.CopyTo(output);
		input.CopyTo(output[Value.Length..]);
		written = input.Length + Value.Length;
	}
}

public sealed class AsciiPrependTransform : AsciiStreamingSpanTransform
{
	public AsciiPrependTransform(char[] value)
	{
		Value = Encoding.ASCII.GetBytes(value);
	}

	public byte[] Value { get; }

	protected override int EstimateOutputBufferSize(int inputLength) => inputLength + Value.Length;

	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		Value.CopyTo(output);
		input.CopyTo(output[Value.Length..]);
		written = input.Length + Value.Length;
	}
}

public sealed class AppendTransform : StreamingSpanTransform
{
	public AppendTransform(char[] value)
	{
		Value = value;
	}

	public char[] Value { get; }

	protected override int EstimateOutputBufferSize(int inputLength) => inputLength + Value.Length;

	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		input.CopyTo(output);
		Value.CopyTo(output[input.Length..]);
		written = input.Length + Value.Length;
	}
}

public sealed class AsciiAppendTransform : AsciiStreamingSpanTransform
{
	public AsciiAppendTransform(char[] value)
	{
		Value = Encoding.ASCII.GetBytes(value);
	}

	public byte[] Value { get; }

	protected override int EstimateOutputBufferSize(int inputLength) => inputLength + Value.Length;

	protected override void Transform(ReadOnlySpan<byte> input, Span<byte> output, out int written)
	{
		input.CopyTo(output);
		Value.CopyTo(output[input.Length..]);
		written = input.Length + Value.Length;
	}
}