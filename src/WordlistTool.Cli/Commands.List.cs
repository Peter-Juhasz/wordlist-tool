using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddList(RootCommand root)
	{
		var main = new Command("list", "Transform list.");
		root.Add(main);

		{
			var countOption = new Option<int>("--count", "Count of entries to take.") { IsRequired = true };

			var filter = new Command("take", "Take a specified number of entries from the beginning.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(countOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new TakeTransform(context.BindingContext.ParseResult.GetValueForOption(countOption));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var countOption = new Option<int>("--count", "Count of entries to take.") { IsRequired = true };

			var filter = new Command("take-last", "Take a specified number of entries from the end.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(countOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new TakeLastTransform(context.BindingContext.ParseResult.GetValueForOption(countOption));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var countOption = new Option<int>("--count", "Count of entries to skip.") { IsRequired = true };

			var filter = new Command("skip", "Skip a specified number of entries from the beginning.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(countOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SkipTransform(context.BindingContext.ParseResult.GetValueForOption(countOption));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var countOption = new Option<int>("--count", "Count of entries to skip.") { IsRequired = true };

			var filter = new Command("skip-last", "Skip a specified number of entries from the end.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(countOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = new SkipLastTransform(context.BindingContext.ParseResult.GetValueForOption(countOption));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}
	}

}