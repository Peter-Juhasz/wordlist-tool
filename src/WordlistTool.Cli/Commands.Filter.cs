using System.CommandLine;
using System.Text.RegularExpressions;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddFilters(RootCommand root)
	{
		var main = new Command("filter", "Filter entries.");
		root.Add(main);

		{
			var sort = new Command("distinct", "Filter out duplicates.");
			sort.AddArgument(inputPathArgument);
			sort.AddArgument(outputPathArgument);
			sort.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption);
				var transform = new DistinctTransform();
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(sort);
		}


		{
			var filter = new Command("whitespace", "Filter whitespace entries.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption);
				var transform = options.CreateTransform(() => new FilterWhitespaceTransform(), ascii: () => new AsciiFilterWhitespaceTransform());
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var regexArgument = new Argument<string>("regex", "Regular expression to match.");

			var filter = new Command("regex", "Filter regular expressions.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddArgument(regexArgument);
			filter.SetHandler(async (context) =>
			{
				var regex = new Regex(context.BindingContext.ParseResult.GetValueForArgument(regexArgument), RegexOptions.Compiled);
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption);
				var transform = new FilterRegexTransform(regex);
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var lengthOption = new Option<int>("--length", "Length to for the filter.") { IsRequired = true };

			var filter = new Command("min-length", "Keep entries longer than a specific length.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(lengthOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption);
				var length = context.BindingContext.ParseResult.GetValueForOption(lengthOption);
				var transform = options.CreateTransform(() => new FilterMinimumLengthTransform(length), ascii: () => new AsciiFilterMinimumLengthTransform(length));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}

		{
			var lengthOption = new Option<int>("--length", "Length to for the filter.") { IsRequired = true };

			var filter = new Command("max-length", "Keep entries no longer than a specific length.");
			filter.AddArgument(inputPathArgument);
			filter.AddArgument(outputPathArgument);
			filter.AddOption(lengthOption);
			filter.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var options = context.GetTransformOptions(inputPathArgument, outputPathArgument, encodingOption, inputEncodingOption, outputEncodingOption);
				var length = context.BindingContext.ParseResult.GetValueForOption(lengthOption);
				var transform = options.CreateTransform(() => new FilterMaximumLengthTransform(length), ascii: () => new AsciiFilterMaximumLengthTransform(length));
				await transform.ExecuteAsync(options.input, options.output, cancellationToken);
			});
			main.AddCommand(filter);
		}
	}
}