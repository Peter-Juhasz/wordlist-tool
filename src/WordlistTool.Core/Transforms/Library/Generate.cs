using WordlistTool.Core.Serialization;

namespace WordlistTool.Core.Transforms.Library;

// TODO: provide fast path for ASCII

public sealed class GenerateTransform : ITransform<GenerateOptions, OutputOptions>
{
	public async Task ExecuteAsync(GenerateOptions input, OutputOptions output, CancellationToken cancellationToken)
	{
		if (input.MaximumLength < input.MinimumLength)
		{
			throw new ArgumentOutOfRangeException(nameof(input.MaximumLength));
		}

		await using var writer = WordlistWriter.GetWriter(output);

		for (int i = input.MinimumLength; i <= input.MaximumLength; i++)
		{
			await GenerateAsync(writer, input.Charset, i, cancellationToken);
		}
	}

	private async Task GenerateAsync(PipeLineWriter writer, char[] charset, int length, CancellationToken cancellationToken)
	{
		char[] state = new char[length];
		int[] indices = new int[length];

		do
		{
			Set(charset, indices, state);
			await writer.WriteAsync(state, cancellationToken);
		} while (Advance(indices, charset.Length));
	}

	private static void Set(char[] charset, int[] indices, char[] state)
	{
		for (int i = 0; i < indices.Length; i++)
		{
			state[i] = charset[indices[i]];
		}
	}

	private static bool Advance(int[] indices, int count)
	{
		for (int i = indices.Length - 1; i >= 0; i--)
		{
			var value = ++indices[i];
			if (value == count)
			{
				indices[i] = 0;

				if (i == 0)
				{
					return false;
				}

				continue;
			}

			return true;
		}

		return false;
	}
}