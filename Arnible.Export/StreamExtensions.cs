using System.IO;
using System.Text;

namespace Arnible.Export
{
  static class StreamExtensions
  {
    private static readonly Encoding EncodingUtf8WithoutBom = new UTF8Encoding(false);
    
    private static FileInfo NormalizeFileExtension(in FileInfo fileInfo, in string expectedExtension)
    {
      if (expectedExtension.ToLowerInvariant() != fileInfo.Extension.ToLowerInvariant())
        return new FileInfo(fileInfo.FullName + expectedExtension);
      else
        return fileInfo;
    }

    private static FileInfo GetTempFile()
    {
      return new FileInfo(Path.GetTempFileName());
    }

    public static FileInfo GetTempFile(in string extension)
    {
      return NormalizeFileExtension(GetTempFile(), extension);
    }

    public static StreamWriter CreateTextWriter(in Stream output)
    {
      return new StreamWriter(output, EncodingUtf8WithoutBom);
    }

    public static StreamWriter CreateTextWriter(in FileInfo info)
    {
      return CreateTextWriter(File.Create(info.FullName));
    }
  }
}