namespace WordlistTool.Core.Transforms;

public class TransformOptions
{
	public TransformOptions(
		InputOptions input,
		OutputOptions output
	)
	{
		Input = input;
		Output = output;
	}

	public InputOptions Input { get; }
	public OutputOptions Output { get; }
}
