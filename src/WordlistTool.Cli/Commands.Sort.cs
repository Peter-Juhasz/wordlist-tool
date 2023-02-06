using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddSort(RootCommand root)
	{
		var main = new Command("sort", "Sort entries.");
		root.Add(main);

		var descendingOption = new Option<bool?>("--descending", "Sort in descending order.");

		{
			var sort = new Command("entries", "Sort entries in a wordlist.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var descending = context.BindingContext.ParseResult.GetValueForOption(descendingOption) ?? false;
				var transform = new SortTransform(descending);
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(sort);
		}

		{
			var sort = new Command("length", "Sort entries by length.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var descending = context.BindingContext.ParseResult.GetValueForOption(descendingOption) ?? false;
				var transform = new SortByLengthTransform(descending);
				await transform.ExecuteAsync(input, output, cancellationToken);
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
				var (input, output) = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SortReverseTransform();
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(sort);
		}
	}
}