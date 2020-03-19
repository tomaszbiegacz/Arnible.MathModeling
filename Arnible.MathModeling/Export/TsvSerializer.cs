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
  public class TsvSerializer<T> : IRecordSerializer<T>
  {
    static readonly ReadOnlyMemory<char> Separator = new[] { TsvConst.SeparatorChar };
    static readonly ReadOnlyMemory<char> NewLine = Environment.NewLine.AsMemory();

    static TextWriter CreateTextWriter(Stream output) => new StreamWriter(output, Encoding.UTF8);

    public static RecordSerializerFileStream<T> ToTempFile() => RecordSerializerFileStream<T>.ToTempFile(new TsvSerializer<T>());

    private readonly IFlatFieldSerializerCollection _serializer;

    public TsvSerializer()
    {
      _serializer = TsvFlatFieldSerializerCollection.For<T>();
    }

    public SerializationMediaType MediaType => SerializationMediaType.TabSeparatedValues;

    public async ValueTask SerializeHeader(Stream output, CancellationToken cancellationToken)
    {
      // stream should be dispose with "output"
      TextWriter writer = CreateTextWriter(output);
      await _serializer.SerializeHeader(writer, Separator, cancellationToken);

      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }

    public async ValueTask SerializeRecord(T record, Stream output, CancellationToken cancellationToken)
    {
      // stream should be disposed with "output"
      TextWriter writer = CreateTextWriter(output);
      await _serializer.SerializeRecord(record, writer, Separator, cancellationToken);

      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }
  }
}
