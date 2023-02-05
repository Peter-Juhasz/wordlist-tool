using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class ZipTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		var first = WordlistReader.ReadStreamingAsync(inputs[0], cancellationToken);
		var set = await WordlistReader.ReadStreamingAsync(inputs[1], cancellationToken).ToHashSetAsync(cancellationToken);

		foreach (var input in inputs.Skip(2))
		{
			await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
			{
				set.Add(line);
			}
		}

		var result = first.Where(e => !set.Contains(e));
		await WordlistWriter.WriteAsync(output, result, cancellationToken);
	}
}