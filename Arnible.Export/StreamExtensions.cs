using System.IO;
using System.Text;

namespace Arnible.Export
{
  static class StreamExtensions
  {
    private static readonly Encoding EncodingUtf8WithoutBom = new UTF8Encoding(false);

    private static FileInfo GetTempFile()
    {
      return new FileInfo(Path.GetTempFileName());
    }

    public static FileInfo GetTempFile(string extension)
    {
      return new FileInfo(GetTempFile().FullName + extension);
    }
  }
}