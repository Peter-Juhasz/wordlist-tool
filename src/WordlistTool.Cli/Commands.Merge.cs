using System.CommandLine;
using WordlistTool.Core.Transforms.Library;

namespace WordlistTool.Cli;

public static partial class Commands
{
	private static void AddMerge(RootCommand root)
	{
		var main = new Command("merge", "Merge entries from multiple lists.");
		root.Add(main);

		var multipleInputArgument = new Option<string[]>("--inputs", "Input paths.") { IsRequired = true, Arity = new(2, 128), AllowMultipleArgumentsPerToken = true };
		var outputArgument = new Option<string>("--output", "Output path.") { IsRequired = true };

		{
			var command = new Command("concatenate", "Concatenate multiple wordlists.");
			command.AddAlias("concat");
			command.AddOption(multipleInputArgument);
			command.AddOption(outputArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (inputs, output) = context.GetTransformOptions(multipleInputArgument, outputArgument, encodingOption);
				var transform = new ConcatTransform();
				await transform.ExecuteAsync(inputs, output, cancellationToken);
			});
			main.AddCommand(command);
		}

		{
			var command = new Command("union", "Union multiple wordlists.");
			command.AddOption(multipleInputArgument);
			command.AddOption(outputArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (inputs, output) = context.GetTransformOptions(multipleInputArgument, outputArgument, encodingOption);
				var transform = new UnionTransform();
				await transform.ExecuteAsync(inputs, output, cancellationToken);
			});
			main.AddCommand(command);
		}

		{
			var separatorArg = new Option<string?>("separator", "Separator to join entries.");

			var command = new Command("zip", "Combine multiple wordlists line by line.");
			command.AddAlias("combine");
			command.AddOption(multipleInputArgument);
			command.AddOption(outputArgument);
			command.AddOption(separatorArg);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var separator = context.BindingContext.ParseResult.GetValueForOption(separatorArg);
				var (inputs, output) = context.GetTransformOptions(multipleInputArgument, outputArgument, encodingOption);
				var transform = new ZipTransform(separator);
				await transform.ExecuteAsync(inputs, output, cancellationToken);
			});
			main.AddCommand(command);
		}

		{
			var command = new Command("except", "Filter out entries in other wordlists from first wordlist.");
			command.AddOption(multipleInputArgument);
			command.AddOption(outputArgument);
			command.SetHandler(async (context) =>
			{
				var cancellationToken = context.GetCancellationToken();
				var (inputs, output) = context.GetTransformOptions(multipleInputArgument, outputArgument, encodingOption);
				var transform = new ExceptTransform();
				await transform.ExecuteAsync(inputs, output, cancellationToken);
			});
			main.AddCommand(command);
		}
	}
}