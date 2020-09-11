using System;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public interface IRecordWriterDisposable<T> : IRecordWriter<T>, IAsyncDisposable
  {
    // intentionally empty
  }

  class RecordWriterDisposable<T> : IRecordWriterDisposable<T>
  {
    private readonly IAsyncDisposable _stream;
    private readonly IRecordWriter<T> _writer;

    public RecordWriterDisposable(IAsyncDisposable stream, IRecordWriter<T> writer)
    {
      _stream = stream;
      _writer = writer;
    }

    public void Write(in T record) => _writer.Write(in record);

    public void WriteNull() => _writer.WriteNull();

    public ValueTask DisposeAsync() => _stream.DisposeAsync();
  }
}