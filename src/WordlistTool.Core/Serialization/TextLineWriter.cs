namespace WordlistTool.Core.Serialization;

public sealed class TextLineWriter : IAsyncDisposable
{
	public TextLineWriter(TextWriter writer)
	{
		Writer = writer;
	}

	public TextWriter Writer { get; }


	public async ValueTask WriteAsync(string line, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await Writer.WriteLineAsync(line);
	}

	public ValueTask DisposeAsync()
	{
		return ((IAsyncDisposable)Writer).DisposeAsync();
	}
}
