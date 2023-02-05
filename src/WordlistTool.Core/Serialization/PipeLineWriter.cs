using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Serialization;

public sealed class PipeLineWriter : IAsyncDisposable
{
	public PipeLineWriter(PipeWriter writer, Encoding encoding, byte[] lineEnding, int bufferSize)
	{
		Writer = writer;
		Encoding = encoding;
		LineEnding = lineEnding;
		BufferSize = bufferSize;
	}

	public PipeWriter Writer { get; }
	public Encoding Encoding { get; }
	public byte[] LineEnding { get; }
	public int BufferSize { get; }

	public ValueTask WriteAsync(string line, CancellationToken cancellationToken)
	{
		Span<byte> bytes = stackalloc byte[Encoding.GetMaxByteCount(line.Length)];
		int written = Encoding.GetBytes(line, bytes);
		return WriteCoreAsync(bytes[..written], cancellationToken);
	}

	public ValueTask WriteAsync(ReadOnlySpan<char> line, CancellationToken cancellationToken)
	{
		Span<byte> bytes = stackalloc byte[Encoding.GetMaxByteCount(line.Length)];
		int written = Encoding.GetBytes(line, bytes);
		return WriteCoreAsync(bytes[..written], cancellationToken);
	}

	private ValueTask WriteCoreAsync(ReadOnlySpan<byte> bytes, CancellationToken cancellationToken)
	{
		var output = Writer.GetSpan(sizeHint: bytes.Length + LineEnding.Length);
		bytes.CopyTo(output);
		LineEnding.CopyTo(output[bytes.Length..]);
		Writer.Advance(bytes.Length + LineEnding.Length);

		if (Writer.UnflushedBytes >= BufferSize)
		{
			// force sync pathways
			return WrapFlushAsync(cancellationToken);
		}

		return default;
	}

	private async ValueTask WrapFlushAsync(CancellationToken cancellationToken)
	{
		await Writer.FlushAsync(cancellationToken);
	}

	public async ValueTask DisposeAsync()
	{
		await Writer.FlushAsync();
		await Writer.CompleteAsync();
	}
}