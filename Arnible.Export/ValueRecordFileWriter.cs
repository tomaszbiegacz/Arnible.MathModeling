using System;
using System.IO;
using System.Threading.Tasks;

namespace Arnible.Export
{
  public interface IValueRecordFileWriter<T> : IValueRecordWriter<T>, IAsyncDisposable where T: struct
  {
    FileInfo Destination { get; }
  }

  class ValueRecordFileWriter<T> : IValueRecordFileWriter<T> where T: struct
  {
    private readonly IAsyncDisposable _stream;
    private readonly IValueRecordWriter<T> _writer;

    public ValueRecordFileWriter(FileInfo destination, Func<ISimpleLogger, IValueRecordWriter<T>> factory)
    {
      Destination = destination;
      var stream = new SimpleLoggerStreamWriter(File.Create(Destination.FullName));
      
      _stream = stream;
      _writer = factory(stream);
    }
    
    public FileInfo Destination { get; }

    public void Write(in T? record) => _writer.Write(in record);
    
    public void Write(in T record) => _writer.Write(in record);

    public ValueTask DisposeAsync() => _stream.DisposeAsync();
  }
}