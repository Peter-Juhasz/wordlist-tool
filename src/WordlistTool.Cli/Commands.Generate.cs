using System.CommandLine;
using System.Text.RegularExpressions;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddGenerate(RootCommand root)
	{
		var main = new Command("generate", "Generate entries.");
		root.Add(main);

		{
			var charsetArg = new Option<string>("--charset", "Charset.");
			var minArg = new Option<int>("--min-length", "Minimum length of entries.");
			minArg.SetDefaultValue(1);
			var maxArg = new Option<int>("--max-length", "Maximum length of entries.");
			maxArg.SetDefaultValue(1);

			var command = new Command("new", "Generate a new list.");
			command.AddArgument(outputPathArgument);
			command.AddOption(charsetArg);
			command.AddOption(minArg);
			command.AddOption(maxArg);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (input, output) = context.GetGenerateTransformOptions(minArg, maxArg, charsetArg, outputPathArgument, encodingOption, outputEncodingOption, lineEndingOption, outputLineEndingOption, bufferSizeOption, outputBufferSizeOption);
				var minimum = context.BindingContext.ParseResult.GetValueForOption(minArg);
				var transform = new GenerateTransform();
				await transform.ExecuteAsync(input, output, cancellationToken);
			});
			main.AddCommand(command);
		}
	}
}