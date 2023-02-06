using System.Globalization;
using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class SplitTransform : ITransform<InputOptions, TemplatedOutputOptions>
{
	public SplitTransform(int chunkSize)
	{
		ChunkSize = chunkSize;
	}

	public int ChunkSize { get; }

	public async Task ExecuteAsync(InputOptions input, TemplatedOutputOptions output, CancellationToken cancellationToken)
	{
		int count = 0;
		int fileCount = 0;
		PipeLineWriter? writer = null;
		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			// open new chunk
			if (count % ChunkSize == 0)
			{
				if (writer != null)
				{
					await using (writer) ;
				}

				fileCount++;
				var current = output.Create(fileCount.ToString(CultureInfo.InvariantCulture));
				writer = WordlistWriter.GetWriter(current);
			}

			// write line
			await writer!.WriteAsync(line, cancellationToken);
			count++;
		}

		if (writer != null)
		{
			await using (writer) ;
		}
	}
}

public sealed class SplitBytesTransform : ITransform<InputOptions, TemplatedOutputOptions>
{
	public SplitBytesTransform(long chunkSize)
	{
		ChunkSize = chunkSize;
	}

	public long ChunkSize { get; }

	public async Task ExecuteAsync(InputOptions input, TemplatedOutputOptions output, CancellationToken cancellationToken)
	{
		long chunkBytes = 0;
		int fileCount = 1;
		OutputOptions? current = output.Create(fileCount.ToString(CultureInfo.InvariantCulture));
		PipeLineWriter? writer = WordlistWriter.GetWriter(current);
		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			var totalLineBytes = input.Encoding.GetByteCount(line) + output.LineEndingBytes.Length;

			// open new chunk
			if (chunkBytes + totalLineBytes >= ChunkSize)
			{
				if (writer != null)
				{
					await using (writer) ;
				}

				fileCount++;
				current = output.Create(fileCount.ToString(CultureInfo.InvariantCulture));
				writer = WordlistWriter.GetWriter(current);
				chunkBytes = 0;
			}

			// write line
			await writer!.WriteAsync(line, cancellationToken);
			chunkBytes += totalLineBytes;
		}

		if (writer != null)
		{
			await using (writer) ;
		}
	}
}