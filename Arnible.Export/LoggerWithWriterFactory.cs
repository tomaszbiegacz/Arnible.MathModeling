using System;
using System.IO;
using Arnible.Export.RecordPerTextRow;

namespace Arnible.Export
{
  public interface ILoggerWithWriterFactory : ISimpleLogger
  {
    IReferenceRecordFileWriter<TRecord> CreateTsvReferenceNotepad<TRecord>(string name) where TRecord: class;
    
    IValueRecordFileWriter<TRecord> CreateTsvValueNotepad<TRecord>(string name) where TRecord: struct;
    
    IRecordFileWriter CreateTsvNotepad(string name);
  }
  
  class LoggerWithWriterFactory : ILoggerWithWriterFactory
  {
    private readonly ISimpleLogger _logger;
    private readonly IRecordWriterBuilder _writerFactory;
    
    public LoggerWithWriterFactory(
      ISimpleLogger logger,
      IRecordWriterBuilder writerFactory)
    {
      _logger = logger;
      _writerFactory = writerFactory;
    }

    public bool IsLoggerEnabled => _logger.IsLoggerEnabled;
    
    public ISimpleLogger Write(in ReadOnlySpan<char> message) => _logger.Write(in message);
    public ISimpleLogger Write(MemoryStream message) => _logger.Write(message);

    public IReferenceRecordFileWriter<TRecord> CreateTsvReferenceNotepad<TRecord>(string name) where TRecord : class
    {
      IReferenceRecordFileWriter<TRecord> writer = new ReferenceRecordFileWriter<TRecord>(
        StreamExtensions.GetTempFile(TsvConst.FileExtension), 
        _writerFactory.CreateTsvReferenceRecordWriter<TRecord>
      );
      
      _logger.Write("Notepad ", name, ": ", writer.Destination.FullName).NewLine();
      return writer;
    }

    public IValueRecordFileWriter<TRecord> CreateTsvValueNotepad<TRecord>(string name) where TRecord : struct
    {
      IValueRecordFileWriter<TRecord> writer = new ValueRecordFileWriter<TRecord>(
        StreamExtensions.GetTempFile(TsvConst.FileExtension), 
        _writerFactory.CreateTsvValueRecordWriter<TRecord>
      );
      
      _logger.Write("Notepad ", name, ": ", writer.Destination.FullName).NewLine();
      return writer;
    }
    
    public IRecordFileWriter CreateTsvNotepad(string name)
    {
      IRecordFileWriter writer = new RecordFileWriter(
        StreamExtensions.GetTempFile(TsvConst.FileExtension), 
        _writerFactory.CreateTsvRecordWriter
      );
      
      _logger.Write("Notepad ", name, ": ", writer.Destination.FullName).NewLine();
      return writer;
    }
  }
}