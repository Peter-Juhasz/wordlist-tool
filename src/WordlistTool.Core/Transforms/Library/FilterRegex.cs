using System.Text.RegularExpressions;

namespace WordlistTool.Core.Transforms.Library;

public sealed class FilterRegexTransform : StreamingSpanTransform
{
	public FilterRegexTransform(Regex regex)
	{
		Regex = regex;
	}

	public Regex Regex { get; }

	protected override void Transform(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		if (Regex.IsMatch(input))
		{
			input.CopyTo(output);
			written = input.Length;
			return;
		}

		written = 0;
	}
}