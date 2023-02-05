using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms;

public abstract class StreamingTransform : ITransform<InputOptions, OutputOptions>
{
	protected abstract IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken);

	public async Task ExecuteAsync(InputOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		var reader = WordlistReader.ReadStreamingAsync(input, cancellationToken);
		var transformed = Transform(reader, cancellationToken);
		await WordlistWriter.WriteAsync(output, transformed, cancellationToken);
	}
}
