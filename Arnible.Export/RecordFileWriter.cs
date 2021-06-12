using System;
using System.IO;
using System.Threading.Tasks;

namespace Arnible.Export
{
  public interface IRecordFileWriter : IRecordWriter, IAsyncDisposable
  {
    FileInfo Destination { get; }
  }
  
  public class RecordFileWriter : IRecordFileWriter
  {
    private readonly IAsyncDisposable _stream;
    private readonly IRecordWriter _writer;

    public RecordFileWriter(FileInfo destination, Func<ISimpleLogger, IRecordWriter> factory)
    {
      Destination = destination;
      var stream = new SimpleLoggerStreamWriter(File.Create(Destination.FullName));
      
      _stream = stream;
      _writer = factory(stream);
    }
    
    public FileInfo Destination { get; }
    
    public IRecordSerializer OpenRecord() => _writer.OpenRecord();

    public ValueTask DisposeAsync() => _stream.DisposeAsync();
  }
}