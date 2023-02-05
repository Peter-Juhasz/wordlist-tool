namespace WordlistTool.Core.Serialization;

public class StringLineWriter : IAsyncDisposable
{
	public StringLineWriter(TextWriter writer)
	{
		Writer = writer;
	}

	public TextWriter Writer { get; }


	public async ValueTask WriteAsync(string line, CancellationToken cancellationToken)
	{
		await Writer.WriteLineAsync(line);
	}

	public ValueTask DisposeAsync()
	{
		return ((IAsyncDisposable)Writer).DisposeAsync();
	}
}