using System.CommandLine;
using System.Text.RegularExpressions;
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
			command.AddAlias("count");
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

		{
			var separatorArg = new Option<long>("--bytes", "Count of entries in a chunk.");

			var command = new Command("bytes", "Split by number of bytes.");
			command.AddArgument(inputPathArgument);
			command.AddArgument(outputPathArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetSplitTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var count = context.BindingContext.ParseResult.GetValueForOption(separatorArg);
				var transform = new SplitBytesTransform(count);
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(command);
		}

		{
			var command = new Command("length", "Split by length.");
			command.AddArgument(inputPathArgument);
			command.AddArgument(outputPathArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetSplitTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SplitLengthTransform();
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(command);
		}

		{
			var regexArgument = new Option<string>("regex", "Regular expression to match.");

			var command = new Command("regex", "Split by regular expression matching.");
			command.AddArgument(inputPathArgument);
			command.AddArgument(outputPathArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetSplitTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var regex = new Regex(context.BindingContext.ParseResult.GetValueForOption(regexArgument), RegexOptions.Compiled);
				var transform = new SplitLengthTransform();
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(command);
		}
	}
}