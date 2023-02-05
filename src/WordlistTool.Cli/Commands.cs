using System.CommandLine;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static readonly Argument<string> inputPathArgument = new Argument<string>("input-path", "Local path of the input wordlist file.");
	private static readonly Argument<string?> outputPathArgument = new Argument<string?>("output-path", "Local path of the output wordlist file.");

	private static readonly Option<string?> encodingOption = new Option<string?>("--encoding", "Default encoding of the wordlist file.");
	private static readonly Option<string?> inputEncodingOption = new Option<string?>("--input-encoding", "Encoding of the input.");
	private static readonly Option<string?> outputEncodingOption = new Option<string?>("--output-encoding", "Encoding of the output.");

	private static readonly Option<string?> lineEndingOption = new Option<string?>("--line-ending", "Default line ending sequence.");
	private static readonly Option<string?> inputLineEndingOption = new Option<string?>("--input-line-ending", "Line ending sequence of input.");
	private static readonly Option<string?> outputLineEndingOption = new Option<string?>("--output-line-ending", "Line ending sequence of output.");

	public static void AddCommands(this RootCommand root)
	{
		encodingOption.SetDefaultValue("ASCII");
		root.AddGlobalOption(encodingOption);
		root.AddGlobalOption(inputEncodingOption);
		root.AddGlobalOption(outputEncodingOption);

		lineEndingOption.SetDefaultValue("0A");
		root.AddGlobalOption(lineEndingOption);
		root.AddGlobalOption(inputLineEndingOption);
		root.AddGlobalOption(outputLineEndingOption);

		AddSort(root);
		AddFilters(root);
		AddTransform(root);
		AddList(root);
		AddExtract(root);
		AddMerge(root);
	}
}