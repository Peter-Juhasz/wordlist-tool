namespace WordlistTool.Core.Transforms;

public interface ITransform<TInputOptions, TOutputOptions>
{
	Task ExecuteAsync(TInputOptions input, TOutputOptions output, CancellationToken cancellationToken);
}