using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Transforms;

public class InputOptions
{
	public InputOptions(
		string? FilePath,
		Stream Stream,
		Encoding Encoding,
		int BufferSize = 16_384
	)
	{
		this.FilePath = FilePath;
		this.Stream = Stream;
		this.Encoding = Encoding;
		this.BufferSize = BufferSize;
	}

	public string? FilePath { get; }
	public Stream Stream { get; }
	public Encoding Encoding { get; }
	public int BufferSize { get; }


	public byte[] LineEndingBytes { get; } = new[] { (byte)'\n' };
	public char[] GetLineEndingChars() => Encoding.GetChars(LineEndingBytes);
	public string GetLineEndingString() => new string(GetLineEndingChars());


	public StreamReader GetStreamReader() => new(Stream, Encoding);

	public PipeReader GetPipeReader() => PipeReader.Create(Stream);

	public TextReader GetTextReader() => new StreamReader(Stream, Encoding);
}