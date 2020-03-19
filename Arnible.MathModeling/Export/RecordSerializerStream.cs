using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public interface IRecordSerializerStream<T> : IAsyncDisposable
  {
    ValueTask Serialize(T record, CancellationToken cancellationToken);
  }

  public class RecordSerializerStream<T> : IRecordSerializerStream<T>
  {
    private bool _isHeaderPrinted;
    private readonly Stream _stream;
    private readonly IRecordSerializer<T> _serializer;

    public RecordSerializerStream(Stream stream, IRecordSerializer<T> serializer)
    {
      _stream = stream ?? throw new ArgumentNullException(nameof(serializer));
      _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
      _isHeaderPrinted = false;
    }

    public uint RecordNumber { get; private set; } = 0;

    public async ValueTask Serialize(T record, CancellationToken cancellationToken)
    {
      if (!_isHeaderPrinted)
      {
        await _serializer.SerializeHeader(_stream, cancellationToken);
        _isHeaderPrinted = true;
      }
      await _serializer.SerializeRecord(record, _stream, cancellationToken);
      RecordNumber++;
    }

    public ValueTask DisposeAsync() => _stream.DisposeAsync();
  }
}
