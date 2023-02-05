namespace WordlistTool.Core.Transforms.Library;

public sealed class TakeTransform : StreamingTransform
{
	public TakeTransform(int count)
	{
		Count = count;
	}

	public int Count { get; }

	protected override IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken) =>
		enumerable.Take(Count);
}

public sealed class TakeLastTransform : StreamingTransform
{
	public TakeLastTransform(int count)
	{
		Count = count;
	}

	public int Count { get; }

	protected override IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken) =>
		enumerable.TakeLast(Count);
}

public sealed class SkipTransform : StreamingTransform
{
	public SkipTransform(int count)
	{
		Count = count;
	}

	public int Count { get; }

	protected override IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken) =>
		enumerable.Skip(Count);
}

public sealed class SkipLastTransform : StreamingTransform
{
	public SkipLastTransform(int count)
	{
		Count = count;
	}

	public int Count { get; }

	protected override IAsyncEnumerable<string> Transform(IAsyncEnumerable<string> enumerable, CancellationToken cancellationToken) =>
		enumerable.SkipLast(Count);
}