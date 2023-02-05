using System.CommandLine;
using System.Text.RegularExpressions;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddExtract(RootCommand root)
	{
		var main = new Command("extract", "Extract entries from other formats.");
		root.Add(main);

		var multipleInputArgument = new Option<string[]>("--inputs", "Input paths.") { IsRequired = true, Arity = new(2, 128), AllowMultipleArgumentsPerToken = true };
		var outputArgument = new Option<string>("--output", "Output path.") { IsRequired = true };

		{
			var regexArgument = new Argument<string?>("regex", "Regular expression to match.");
			regexArgument.SetDefaultValue(@"\w+");

			var command = new Command("regex", "Extract entries using a regular expression.");
			command.AddOption(multipleInputArgument);
			command.AddOption(outputArgument);
			command.AddArgument(regexArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (inputs, output) = context.GetTransformOptions(multipleInputArgument, outputArgument, encodingOption, inputEncodingOption, outputEncodingOption, lineEndingOption, inputLineEndingOption, outputLineEndingOption, bufferSizeOption, inputBufferSizeOption, outputBufferSizeOption);
				var regex = new Regex(context.BindingContext.ParseResult.GetValueForArgument(regexArgument)!, RegexOptions.Compiled);
				var transform = new RegexExtractTransform(regex);
				await transform.ExecuteAsync(inputs, output, cancellationToken);
			});
			main.AddCommand(command);
		}
	}
}