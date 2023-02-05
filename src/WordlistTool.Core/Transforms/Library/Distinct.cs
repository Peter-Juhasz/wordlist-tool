namespace WordlistTool.Core.Transforms.Library;

public class DistinctTransform : StreamingTransform
{
	protected override IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken) =>
		enumerable.Distinct();
}
