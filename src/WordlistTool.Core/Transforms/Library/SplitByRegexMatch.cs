using System.Text.RegularExpressions;
using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class SplitByRegexMatchTransform : ITransform<InputOptions, TemplatedOutputOptions>
{
	public SplitByRegexMatchTransform(Regex regex)
	{
		Regex = regex;
	}

	public Regex Regex { get; }

	public async Task ExecuteAsync(InputOptions input, TemplatedOutputOptions output, CancellationToken cancellationToken)
	{
		await using var matching = WordlistWriter.GetWriter(output.Create("matching"));
		await using var nonMatching = WordlistWriter.GetWriter(output.Create("non-matching"));

		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			var writer = Regex.IsMatch(line) ? matching : nonMatching;
			await writer.WriteAsync(line, cancellationToken);
		}
	}
}