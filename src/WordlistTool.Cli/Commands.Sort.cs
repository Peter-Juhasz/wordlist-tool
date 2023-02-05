using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddSort(RootCommand root)
	{
		var main = new Command("sort", "Sort entries.");
		root.Add(main);

		{
			var sort = new Command("asc", "Sort entries in a wordlist.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SortTransform(false);
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(sort);
		}

		{
			var sort = new Command("desc", "Sort entries in a wordlist in a descending order.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SortTransform(true);
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(sort);
		}

		{
			var sort = new Command("reverse", "Reverse the order of entries in a wordlist.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SortReverseTransform();
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(sort);
		}
	}

}