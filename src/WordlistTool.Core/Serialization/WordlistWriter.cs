using System.IO.Pipelines;
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
		var writer = PipeWriter.Create(stream, new(null, bufferSize, false));
		await WriteAsync(writer, bufferSize, encoding, lineEnding, list, cancellationToken);
	}

	public static async Task WriteAsync(PipeWriter pipe, int bufferSize, Encoding encoding, byte[] lineEnding, IEnumerable<string> list, CancellationToken cancellationToken)
	{
		await using var writer = GetWriter(pipe, bufferSize, encoding, lineEnding);
		foreach (var line in list)
		{
			await writer.WriteAsync(line, cancellationToken);
		}
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
		var writer = PipeWriter.Create(stream, new(null, bufferSize, false));
		await WriteAsync(writer, bufferSize, encoding, lineEnding, list, cancellationToken);
	}

	public static async Task WriteAsync(PipeWriter pipe, int bufferSize, Encoding encoding, byte[] lineEnding, IAsyncEnumerable<string> list, CancellationToken cancellationToken)
	{
		await using var writer = GetWriter(pipe, bufferSize, encoding, lineEnding);
		await foreach (var line in list)
		{
			await writer.WriteAsync(line, cancellationToken);
		}
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


	public static TextLineWriter GetWriter(TextWriter writer) => new(writer);

	public static PipeLineWriter GetWriter(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding)
	{
		var pipe = PipeWriter.Create(stream, new(null, bufferSize, false));
		return GetWriter(pipe, bufferSize, encoding, lineEnding);
	}

	public static PipeLineWriter GetWriter(PipeWriter pipe, int bufferSize, Encoding encoding, byte[] lineEnding)
	{
		return new(pipe, encoding, lineEnding, bufferSize); // TODO: revisit disposal of streams
	}

	public static PipeLineWriter GetWriter(OutputOptions output)
	{
		return GetWriter(output.GetPipeWriter(), output.BufferSize, output.Encoding, output.LineEndingBytes);
	}

#pragma warning disable CS1998 // TODO: should be async eventually
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
	public static async Task<PipeLineWriter> GetWriterAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, CancellationToken cancellationToken)
	{
		var stream = File.Create(filePath);
		return GetWriter(stream, bufferSize, encoding, lineEnding);
	}
}
