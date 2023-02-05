using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Transforms;

public class OutputOptions
{
	public OutputOptions(
		string? FilePath,
		Stream Stream,
		Encoding Encoding,
		byte[] LineEnding,
		int BufferSize = 16_384
	)
	{
		this.FilePath = FilePath;
		this.Stream = Stream;
		this.Encoding = Encoding;
		this.LineEndingBytes = LineEnding;
		this.BufferSize = BufferSize;
	}

	public string? FilePath { get; }
	public Stream Stream { get; }
	public Encoding Encoding { get; }
	public int BufferSize { get; }


	public byte[] LineEndingBytes { get; } = new[] { (byte)'\n' };
	public char[] GetLineEndingChars() => Encoding.GetChars(LineEndingBytes);
	public string GetLineEndingString() => new string(GetLineEndingChars());


	public TextWriter GetTextWriter() => new StreamWriter(Stream, Encoding, BufferSize);
	public StreamWriter GetStreamWriter() => new(Stream, Encoding, BufferSize);
	public PipeWriter GetPipeWriter() => PipeWriter.Create(Stream, new(null, BufferSize, false));
}