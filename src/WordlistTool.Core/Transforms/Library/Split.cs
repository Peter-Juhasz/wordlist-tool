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
		int fileCount = 1;
		OutputOptions? current = null;
		PipeLineWriter? writer = null;
		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			if (count % ChunkSize == 0)
			{
				if (writer != null)
				{
					await using (writer) ;
				}

				current = output.Create(fileCount.ToString(CultureInfo.InvariantCulture));
				writer = WordlistWriter.GetWriter(current);
			}

			await writer!.WriteAsync(line, cancellationToken);
			count++;
		}

		if (writer != null)
		{
			await using (writer) ;
		}
	}
}