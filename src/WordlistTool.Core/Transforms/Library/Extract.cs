using System.Text.RegularExpressions;
using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class RegexExtractTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public RegexExtractTransform(Regex regex)
	{
		Regex = regex;
	}

	public Regex Regex { get; }

	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		await using var writer = WordlistWriter.GetWriter(output);
		foreach (var input in inputs)
		{
			using var reader = input.GetTextReader();
			var text = await reader.ReadToEndAsync(cancellationToken);
			foreach (Match match in Regex.Matches(text))
			{
				await writer.WriteAsync(match.Value, cancellationToken);
			}
		}
	}
}