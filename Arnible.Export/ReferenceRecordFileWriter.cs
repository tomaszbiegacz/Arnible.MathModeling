using System;
using System.IO;
using System.Threading.Tasks;

namespace Arnible.Export
{
  public interface IReferenceRecordFileWriter<TRecord> : IReferenceRecordWriter<TRecord>, IAsyncDisposable where TRecord: class?
  {
    FileInfo Destination { get; }
  }

  class ReferenceRecordFileWriter<TRecord> : IReferenceRecordFileWriter<TRecord> where TRecord: class?
  {
    private readonly IAsyncDisposable _stream;
    private readonly IReferenceRecordWriter<TRecord> _writer;

    public ReferenceRecordFileWriter(FileInfo destination, Func<ISimpleLogger, IReferenceRecordWriter<TRecord>> factory)
    {
      Destination = destination;
      var stream = new SimpleLoggerStreamWriter(File.Create(Destination.FullName));
      
      _stream = stream;
      _writer = factory(stream);
    }
    
    public FileInfo Destination { get; }

    public void Write(TRecord record) => _writer.Write(record);

    public ValueTask DisposeAsync() => _stream.DisposeAsync();
  }
}