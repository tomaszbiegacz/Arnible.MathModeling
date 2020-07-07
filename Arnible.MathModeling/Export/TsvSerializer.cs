using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  /// <summary>
  /// TSV file that can be open in Excel
  /// </summary>
  /// <remarks>
  /// https://en.wikipedia.org/wiki/Tab-separated_values
  /// </remarks>
  public class TsvSerializer<T> : IRecordSerializer<T> where T: struct
  {
    static readonly Encoding EncodingUtf8WithoutBom = new UTF8Encoding(false);
    static readonly ReadOnlyMemory<char> Separator = new[] { TsvConst.SeparatorChar };
    static readonly ReadOnlyMemory<char> NewLine = "\n".AsMemory();

    static TextWriter CreateTextWriter(in Stream output) => new StreamWriter(output, EncodingUtf8WithoutBom);

    public static RecordSerializerFileStream<T> ToTempFile() => RecordSerializerFileStream<T>.ToTempFile(new TsvSerializer<T>());

    private readonly IFlatFieldSerializerCollection _serializer;

    public TsvSerializer()
    {
      _serializer = TsvFlatFieldSerializerCollection.For<T>();
    }

    public SerializationMediaType MediaType => SerializationMediaType.TabSeparatedValues;

    public async Task SerializeHeader(Stream output, CancellationToken cancellationToken)
    {
      // stream should be dispose with "output"
      TextWriter writer = CreateTextWriter(output);
      await _serializer.SerializeHeader(writer, Separator, cancellationToken);

      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }

    public async Task SerializeRecord(T record, Stream output, CancellationToken cancellationToken)
    {
      // stream should be disposed with "output"
      TextWriter writer = CreateTextWriter(output);
      await _serializer.SerializeRecord(record, writer, Separator, cancellationToken);

      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }
  }
}
