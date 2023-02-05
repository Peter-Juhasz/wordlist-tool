using System.CommandLine;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static readonly Argument<string> inputPathArgument = new Argument<string>("input-path", "Local path of the input wordlist file.");
	private static readonly Argument<string?> outputPathArgument = new Argument<string?>("output-path", "Local path of the output wordlist file.");
	private static readonly Option<string?> encodingOption = new Option<string?>("--encoding", "Encoding of the wordlist file.");
	private static readonly Option<string?> lineEndingOption = new Option<string?>("--line-ending", "Line ending sequence.");

	public static void AddCommands(this RootCommand root)
	{
		encodingOption.SetDefaultValue("ASCII");
		lineEndingOption.SetDefaultValue("LF");
		root.AddGlobalOption(encodingOption);
		root.AddGlobalOption(lineEndingOption);

		AddSort(root);
		AddFilters(root);
		AddTransform(root);
		AddList(root);
		AddExtract(root);
		AddMerge(root);
	}
}