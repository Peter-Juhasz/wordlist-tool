using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddTransform(RootCommand root)
	{
		var main = new Command("transform", "Transform entries.");
		root.Add(main);

		{
			var filter = new Command("to-upper", "Convert entries to uppercase.");
			filter.AddAlias("upper");
			filter.AddAlias("uppercase");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = options.CreateTransform(() => new ToUpperTransform(), ascii: () => new ToUpperTransform());
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var filter = new Command("to-lower", "Convert entries to lowercase.");
			filter.AddAlias("lower");
			filter.AddAlias("lowercase");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = options.CreateTransform(() => new ToLowerTransform(), ascii: () => new ToLowerTransform());
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var valueOption = new Argument<string>("value", "String to prepend.");

			var filter = new Command("prepend", "Prepend to entries.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddArgument(valueOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var value = context.BindingContext.ParseResult.GetValueForArgument(valueOption).ToCharArray();
				var transform = options.CreateTransform(() => new PrependTransform(value), ascii: () => new AsciiPrependTransform(value));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var valueOption = new Argument<string>("value", "String to prepend.");

			var filter = new Command("append", "Append to entries.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddArgument(valueOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var value = context.BindingContext.ParseResult.GetValueForArgument(valueOption).ToCharArray();
				var transform = options.CreateTransform(() => new AppendTransform(value), ascii: () => new AsciiAppendTransform(value));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var filter = new Command("reverse", "Reverse the characters of entries.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = options.CreateTransform(() => new ReverseTransform(), ascii: () => new AsciiReverseTransform());
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var filter = new Command("trim", "Trim leading and trailing whitespace of entries.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var transform = options.CreateTransform(() => new TrimTransform(), ascii: () => new TrimTransform());
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}
	}

}