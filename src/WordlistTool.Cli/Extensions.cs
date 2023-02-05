using Microsoft.Extensions.FileSystemGlobbing;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using WordlistTool.Core.Transforms;

public static partial class Extensions
{
	public static InputOptions GetInputOptions(
		this InvocationContext context,
		Argument<string> inputArg,
		Option<string?> encodingOption
	)
	{
		var inputPath = context.BindingContext.ParseResult.GetValueForArgument(inputArg);
		var input = ResolveInputStream(inputPath);

		return new(inputPath, input, context.GetEncoding(encodingOption));
	}

	private static Stream ResolveInputStream(string inputPath) => inputPath switch
	{
		"IN" => Console.OpenStandardInput(),
		_ => File.OpenRead(inputPath)
	};

	public static IReadOnlyList<InputOptions> GetInputOptions(
		this InvocationContext context,
		Option<string[]> inputArg,
		Option<string?> encodingOption
	)
	{
		var arguments = context.BindingContext.ParseResult.GetValueForOption(inputArg)!;

		var paths = new List<string>();
		foreach (var arg in arguments)
		{
			if (arg is "IN")
			{
				paths.Add(arg);
				continue;
			}

			var matcher = new Matcher();
			matcher.AddInclude(arg);
			paths.AddRange(matcher.GetResultsInFullPath(Environment.CurrentDirectory));
		}

		return paths
			.Select(i => new InputOptions(i, ResolveInputStream(i), context.GetEncoding(encodingOption)))
			.ToList();
	}

	public static OutputOptions GetOutputOptions(
		this InvocationContext context,
		Argument<string> outputArg,
		Option<string?> encodingOption
	)
	{
		var outputPath = context.BindingContext.ParseResult.GetValueForArgument(outputArg);
		var output = ResolveOutputStream(outputPath);

		return new(outputPath, output, context.GetEncoding(encodingOption));
	}

	public static OutputOptions GetOutputOptions(
		this InvocationContext context,
		Option<string> outputArg,
		Option<string?> encodingOption
	)
	{
		var outputPath = context.BindingContext.ParseResult.GetValueForOption(outputArg);
		var output = ResolveOutputStream(outputPath);

		return new(outputPath, output, context.GetEncoding(encodingOption));
	}

	private static Stream ResolveOutputStream(string outputPath)
	{
		return outputPath switch
		{
			"OUT" => Console.OpenStandardOutput(),
			_ => File.Create(outputPath)
		};
	}

	public static Encoding GetEncoding(
		this InvocationContext context,
		Option<string?> encodingOption
	) => context.BindingContext.ParseResult.GetValueForOption(encodingOption) switch
	{
		string e => Encoding.GetEncoding(e),
		_ => Encoding.ASCII
	};

	public static (InputOptions input, OutputOptions output) GetTransformOptions(this InvocationContext context,
		Argument<string> inputArg, Argument<string> outputArg,
		Option<string?> encodingOption
	) => (
		context.GetInputOptions(inputArg, encodingOption),
		context.GetOutputOptions(outputArg, encodingOption)
	);

	public static (IReadOnlyList<InputOptions> inputs, OutputOptions output) GetTransformOptions(this InvocationContext context,
		Option<string[]> inputArg, Option<string> outputArg,
		Option<string?> encodingOption
	) => (
		context.GetInputOptions(inputArg, encodingOption),
		context.GetOutputOptions(outputArg, encodingOption)
	);

	public static ITransform<InputOptions, OutputOptions> CreateTransform(
		this (InputOptions input, OutputOptions output) options,
		Func<ITransform<InputOptions, OutputOptions>> general,
		Func<ITransform<InputOptions, OutputOptions>> ascii
	) =>
		(options.input.Encoding == Encoding.ASCII ? ascii : general)();

	public static IEqualityComparer<string> GetComparer(this InvocationContext context, Option<string?> comparer) => context.BindingContext.ParseResult.GetValueForOption(comparer) switch
	{
		_ => StringComparer.Ordinal
	};

	public static bool GetDescending(this InvocationContext context, Option<bool?> descending) => context.BindingContext.ParseResult.GetValueForOption(descending) ?? false;
}