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

	public static async Task WriteAsync(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		using var writer = new StreamWriter(stream, encoding, bufferSize) { NewLine = encoding.GetString(lineEnding) };
		await WriteAsync(writer, list, cancellationToken);
	}

	public static async Task WriteAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		await using var stream = File.Create(filePath, bufferSize);
		await WriteAsync(stream, bufferSize, encoding, lineEnding, list, cancellationToken);
	}

	public static async Task WriteAsync(OutputOptions output, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		await WriteAsync(output.Stream, output.BufferSize, output.Encoding, output.LineEndingBytes, list, cancellationToken);
	}


	public static async Task WriteAsync(TextWriter writer, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await foreach (var item in list)
		{
			await writer.WriteLineAsync(item);
		}
	}

	public static async Task WriteAsync(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		using var writer = new StreamWriter(stream, encoding, bufferSize) { NewLine = encoding.GetString(lineEnding) };
		await WriteAsync(writer, list, cancellationToken);
	}

	public static async Task WriteAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		await using var stream = File.Create(filePath, bufferSize);
		await WriteAsync(stream, bufferSize, encoding, lineEnding, list, cancellationToken);
	}

	public static async Task WriteAsync(OutputOptions output, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		await WriteAsync(output.Stream, output.BufferSize, output.Encoding, output.LineEndingBytes, list, cancellationToken);
	}


	public static StringLineWriter GetWriterAsync(TextWriter writer) => new(writer);

	public static StringLineWriter GetWriterAsync(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding)
	{
		var streamWriter = new StreamWriter(stream, encoding, bufferSize) { NewLine = encoding.GetString(lineEnding) };
		return new(streamWriter);
	}

	public static StringLineWriter GetWriterAsync(OutputOptions output)
	{
		return GetWriterAsync(output.Stream, output.BufferSize, output.Encoding, output.LineEndingBytes);
	}

	public static async Task<StringLineWriter> GetWriterAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, CancellationToken cancellationToken)
	{
		var stream = File.Create(filePath);
		return GetWriterAsync(stream, bufferSize, encoding, lineEnding);
	}
}
