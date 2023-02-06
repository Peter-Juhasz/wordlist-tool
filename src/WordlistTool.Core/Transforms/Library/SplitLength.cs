using System.Globalization;
using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

public sealed class SplitLengthTransform : ITransform<InputOptions, TemplatedOutputOptions>
{
	public async Task ExecuteAsync(InputOptions input, TemplatedOutputOptions output, CancellationToken cancellationToken)
	{
		Dictionary<int, PipeLineWriter> writers = new();

		await foreach (var line in WordlistReader.ReadStreamingAsync(input, cancellationToken))
		{
			var length = line.Length;

			if (!writers.TryGetValue(length, out var writer))
			{
				writer = WordlistWriter.GetWriter(output.Create(length.ToString(CultureInfo.InvariantCulture)));
				writers.Add(length, writer);
			}

			await writer.WriteAsync(line, cancellationToken);
		}

		foreach (var writer in writers.Values)
		{
			await using (writer) ;
		}
	}
}