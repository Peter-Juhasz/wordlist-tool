using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

// TODO: provide fast path for ASCII, and span based combination

public sealed class ZipTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public ZipTransform(string? separator)
	{
		Separator = separator;
	}

	public string? Separator { get; }

	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		var first = WordlistReader.ReadStreamingAsync(inputs[0], cancellationToken);
		var result = first.Zip(WordlistReader.ReadStreamingAsync(inputs[1], cancellationToken), (left, right) => left + Separator + right);

		foreach (var input in inputs.Skip(2))
		{
			result = result.Zip(WordlistReader.ReadStreamingAsync(input, cancellationToken), (left, right) => left + Separator + right);
		}

		await WordlistWriter.WriteAsync(output, result, cancellationToken);
	}
}