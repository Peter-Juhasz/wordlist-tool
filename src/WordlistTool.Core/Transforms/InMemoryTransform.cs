using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms;

public abstract class InMemoryTransform : ITransform<InputOptions, OutputOptions>
{
	protected abstract void Transform(IList<string> list, CancellationToken cancellationToken);

	public async Task ExecuteAsync(InputOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		var list = await WordlistReader.ReadToMemoryAsync(input, cancellationToken);
		Transform(list, cancellationToken);
		await WordlistWriter.WriteAsync(output, list, cancellationToken);
	}
}
