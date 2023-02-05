using System.Runtime.CompilerServices;
using System.Text;
using WordlistTool.Core.Transforms;

namespace WordlistTool.Core.Serialization;

public static class WordlistReader
{
	public static async IAsyncEnumerable<string> ReadStreamingAsync(TextReader reader, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		while (await reader.ReadLineAsync(cancellationToken) is string line)
		{
			yield return line;
		}
	}

	public static async IAsyncEnumerable<string> ReadStreamingAsync(Stream stream, Encoding encoding, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: false); // TODO: new line not supported
		await foreach (var line in ReadStreamingAsync(reader, cancellationToken)) // TODO: rewrite to pipelines, because textreader is slow
		{
			yield return line;
		}
	}

	public static async IAsyncEnumerable<string> ReadStreamingAsync(string filePath, Encoding encoding, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await using var stream = File.OpenRead(filePath);
		await foreach (var line in ReadStreamingAsync(stream, encoding, cancellationToken))
		{
			yield return line;
		}
	}

	public static IAsyncEnumerable<string> ReadStreamingAsync(InputOptions input, CancellationToken cancellationToken) =>
		ReadStreamingAsync(input.Stream, input.Encoding, cancellationToken);


	public static async Task<IList<string>> ReadToMemoryAsync(string filePath, Encoding encoding, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(filePath, encoding, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(Stream stream, Encoding encoding, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(stream, encoding, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(TextReader stream, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(stream, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(InputOptions input, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(input.Stream, input.Encoding, cancellationToken).ToListAsync(cancellationToken);
	}
}
