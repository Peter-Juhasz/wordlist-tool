using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using WordlistTool.Core.Transforms;

namespace WordlistTool.Core.Serialization;

public static class WordlistReader
{
	private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, byte[] lineEnding, out ReadOnlySequence<byte> line)
	{
		// try to find next line separator
		var reader = new SequenceReader<byte>(buffer);
		if (reader.TryReadTo(out line, lineEnding, advancePastDelimiter: false))
		{
			buffer = buffer.Slice(line.Length + lineEnding.Length);
			return true;
		}

		line = default;
		return false;
	}

	public static async IAsyncEnumerable<string> ReadStreamingAsync(TextReader reader, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		while (await reader.ReadLineAsync(cancellationToken) is string line)
		{
			yield return line;
		}
	}

	public static IAsyncEnumerable<string> ReadStreamingAsync(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var pipe = PipeReader.Create(stream, new(null, bufferSize, bufferSize, false));
		return ReadStreamingAsync(pipe, encoding, lineEnding, cancellationToken);
	}

	public static async IAsyncEnumerable<string> ReadStreamingAsync(PipeReader pipe, Encoding encoding, byte[] lineEnding, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		while (true)
		{
			// try read
			var result = await pipe.ReadAsync(cancellationToken);
			if (result.IsCanceled)
			{
				break;
			}

			// try find individual lines
			ReadOnlySequence<byte> buffer = result.Buffer;

			while (TryReadLine(ref buffer, lineEnding, out ReadOnlySequence<byte> lineBytes))
			{
				var line = encoding.GetString(lineBytes);
				yield return line;
			}

			// advance reader
			pipe.AdvanceTo(buffer.Start, buffer.End);

			// check whether we are at the end of the stream
			if (result.IsCompleted)
			{
				break;
			}
		}
	}

	public static async IAsyncEnumerable<string> ReadStreamingAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await using var stream = File.OpenRead(filePath);
		await foreach (var line in ReadStreamingAsync(stream, bufferSize, encoding, lineEnding, cancellationToken))
		{
			yield return line;
		}
	}

	public static IAsyncEnumerable<string> ReadStreamingAsync(InputOptions input, CancellationToken cancellationToken) =>
		ReadStreamingAsync(input.GetPipeReader(), input.Encoding, input.LineEndingBytes, cancellationToken);


	public static async Task<IList<string>> ReadToMemoryAsync(string filePath, int bufferSize, Encoding encoding, byte[] lineEnding, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(filePath, bufferSize, encoding, lineEnding, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(Stream stream, int bufferSize, Encoding encoding, byte[] lineEnding, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(stream, bufferSize, encoding, lineEnding, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(TextReader stream, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(stream, cancellationToken).ToListAsync(cancellationToken);
	}

	public static async Task<IList<string>> ReadToMemoryAsync(InputOptions input, CancellationToken cancellationToken)
	{
		return await ReadStreamingAsync(input.GetPipeReader(), input.Encoding, input.LineEndingBytes, cancellationToken).ToListAsync(cancellationToken);
	}
}
