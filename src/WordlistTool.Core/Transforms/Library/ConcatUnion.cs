using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class ConcatTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		var concatenated = AsyncEnumerable.Concat(
			WordlistReader.ReadStreamingAsync(inputs[0], cancellationToken),
			WordlistReader.ReadStreamingAsync(inputs[1], cancellationToken)
		);

		foreach (var input in inputs.Skip(2))
		{
			concatenated = concatenated.Concat(WordlistReader.ReadStreamingAsync(input, cancellationToken));
		}

		await WordlistWriter.WriteAsync(output, concatenated, cancellationToken);
	}
}

public sealed class UnionTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		var concatenated = AsyncEnumerable.Union(
			WordlistReader.ReadStreamingAsync(inputs[0], cancellationToken),
			WordlistReader.ReadStreamingAsync(inputs[1], cancellationToken)
		);

		foreach (var input in inputs.Skip(2))
		{
			concatenated = concatenated.Union(WordlistReader.ReadStreamingAsync(input, cancellationToken));
		}

		await WordlistWriter.WriteAsync(output, concatenated, cancellationToken);
	}
}