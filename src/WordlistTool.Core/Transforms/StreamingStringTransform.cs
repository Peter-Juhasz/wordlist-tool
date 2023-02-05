using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms;

public abstract class StreamingStringTransform : ITransform<InputOptions, OutputOptions>
{
	protected abstract string Transform(string line, CancellationToken cancellationToken);

	public async Task ExecuteAsync(InputOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		await using var writer = WordlistWriter.GetWriter(output);
		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			var transformed = Transform(line, cancellationToken);
			if (!string.IsNullOrWhiteSpace(transformed))
			{
				await writer.WriteAsync(transformed, cancellationToken);
			}
		}
	}
}
