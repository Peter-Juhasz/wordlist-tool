using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddSplit(RootCommand root)
	{
		var main = new Command("split", "Split entries into multiple lists.");
		root.Add(main);

		{
			var separatorArg = new Option<int>("--count", "Count of entries in a chunk.");

			var command = new Command("entries", "Split by number of entries.");
			command.AddArgument(inputPathArgument);
			command.AddArgument(outputPathArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetSplitTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var count = context.BindingContext.ParseResult.GetValueForOption(separatorArg);
				var transform = new SplitTransform(count);
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(command);
		}
	}
}