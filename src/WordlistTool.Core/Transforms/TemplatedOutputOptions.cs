using System.Text;

namespace WordlistTool.Core.Transforms;

public class TemplatedOutputOptions
{
	public TemplatedOutputOptions(
		string workingDirectory,
		string filePathTemplate,
		int bufferSize,
		Encoding encoding,
		byte[] lineEndingBytes
	)
	{
		WorkingDirectory = workingDirectory;
		FilePathTemplate = filePathTemplate;
		BufferSize = bufferSize;
		Encoding = encoding;
		LineEndingBytes = lineEndingBytes;
	}

	public string WorkingDirectory { get; }
	public string FilePathTemplate { get; }
	public int BufferSize { get; }
	public Encoding Encoding { get; }
	public byte[] LineEndingBytes { get; }

	public OutputOptions Create(string value) => new(
		String.Format(FilePathTemplate, value),
		File.Create(value),
		Encoding,
		LineEndingBytes,
		BufferSize
	);
}
