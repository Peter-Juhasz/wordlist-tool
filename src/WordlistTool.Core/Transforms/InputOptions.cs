using System.IO.Pipelines;
using System.Text;

namespace WordlistTool.Core.Transforms;

public class InputOptions
{
	public InputOptions(
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


	public StreamReader GetStreamReader() => new(Stream, Encoding, detectEncodingFromByteOrderMarks: false, BufferSize);

	public PipeReader GetPipeReader() => PipeReader.Create(Stream, new(null, BufferSize, BufferSize, false));

	public TextReader GetTextReader() => new StreamReader(Stream, Encoding, detectEncodingFromByteOrderMarks: false, BufferSize);
}