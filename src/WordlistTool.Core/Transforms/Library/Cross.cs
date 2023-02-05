using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

// TODO: provide fast path for ASCII, and span based combination
// TODO: stream second list?

public sealed class CrossTransform : ITransform<IReadOnlyList<InputOptions>, OutputOptions>
{
	public CrossTransform(string? separator)
	{
		Separator = separator;
	}

	public string? Separator { get; }

	public async Task ExecuteAsync(IReadOnlyList<InputOptions> inputs, OutputOptions output, CancellationToken cancellationToken)
	{
		if (inputs is { Count: not 1 or 2 })
		{
			throw new NotSupportedException();
		}

		var first = WordlistReader.ReadStreamingAsync(inputs[0], cancellationToken);
		var second = await WordlistReader.ReadToMemoryAsync(inputs.Count < 2 ? inputs[0] : inputs[1], cancellationToken);

		var writer = WordlistWriter.GetWriterAsync(output);

		await foreach (var line in first)
		{
			foreach (var subject in second)
			{
				var result = line + Separator + subject;
				await writer.WriteAsync(result, cancellationToken);
			}
		}
	}
}