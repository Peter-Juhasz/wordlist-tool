namespace WordlistTool.Core.Transforms;

public class GenerateOptions
{
	public GenerateOptions(
		char[] charset,
		int minimumLength,
		int maximumLength
	)
	{
		Charset = charset;
		MinimumLength = minimumLength;
		MaximumLength = maximumLength;
	}

	public char[] Charset { get; }
	public int MinimumLength { get; }
	public int MaximumLength { get; }
}
