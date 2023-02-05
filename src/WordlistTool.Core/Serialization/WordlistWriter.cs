using System.Text;
using WordlistTool.Core.Transforms;

namespace WordlistTool.Core.Serialization;

public static class WordlistWriter
{
	public static async Task WriteAsync(TextWriter writer, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		foreach (var item in list)
		{
			await writer.WriteLineAsync(item);
		}
	}

	public static async Task WriteAsync(Stream stream, Encoding encoding, IEnumerable<string> list, int bufferSize, CancellationToken cancellationToken)
	{
		using var writer = new StreamWriter(stream, encoding, bufferSize);
		await WriteAsync(writer, list, cancellationToken);
	}

	public static async Task WriteAsync(string filePath, Encoding encoding, IEnumerable<string> list, int bufferSize, CancellationToken cancellationToken)
	{
		await using var stream = File.Create(filePath, bufferSize);
		await WriteAsync(stream, encoding, list, bufferSize, cancellationToken);
	}

	public static async Task WriteAsync(OutputOptions output, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		await WriteAsync(output.Stream, output.Encoding, list, output.BufferSize, cancellationToken);
	}


	public static async Task WriteAsync(TextWriter writer, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await foreach (var item in list)
		{
			await writer.WriteLineAsync(item);
		}
	}

	public static async Task WriteAsync(Stream stream, Encoding encoding, IAsyncEnumerable<string> list, int bufferSize, CancellationToken cancellationToken)
	{
		using var writer = new StreamWriter(stream, encoding, bufferSize);
		await WriteAsync(writer, list, cancellationToken);
	}

	public static async Task WriteAsync(string filePath, Encoding encoding, IAsyncEnumerable<string> list, int bufferSize, CancellationToken cancellationToken)
	{
		await using var stream = File.Create(filePath, bufferSize);
		await WriteAsync(stream, encoding, list, bufferSize, cancellationToken);
	}

	public static async Task WriteAsync(OutputOptions output, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		await WriteAsync(output.Stream, output.Encoding, list, output.BufferSize, cancellationToken);
	}


	public static StringLineWriter GetWriterAsync(TextWriter writer) => new(writer);

	public static StringLineWriter GetWriterAsync(Stream stream, Encoding encoding, int bufferSize)
	{
		var streamWriter = new StreamWriter(stream, encoding, bufferSize);
		return new(streamWriter);
	}

	public static StringLineWriter GetWriterAsync(OutputOptions output)
	{
		return GetWriterAsync(output.Stream, output.Encoding, output.BufferSize);
	}

	public static async Task<StringLineWriter> GetWriterAsync(string filePath, Encoding encoding, int bufferSize, CancellationToken cancellationToken)
	{
		var stream = File.Create(filePath);
		return GetWriterAsync(stream, encoding, bufferSize);
	}
}
