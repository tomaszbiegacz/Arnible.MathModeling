using System;
using Arnible.Logger;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowStream
  {
    private readonly ValuesRowWriter _writer;
    private readonly SimpleLoggerMemoryWriter _secondaryLogger;
    private readonly ValuesRowWriter _secondaryWriter;

    public RecordPerRowStream(
      string rowFieldSeparator,
      ISimpleLogger logger)
    {
      _writer = new ValuesRowWriter(logger, rowFieldSeparator);
      
      _secondaryLogger = new SimpleLoggerMemoryWriter();
      _secondaryWriter = new ValuesRowWriter(_secondaryLogger, rowFieldSeparator);
      
      IsHeaderSerialized = false;
    }

    public bool IsHeaderSerialized { get; private set; }
    
    public void CommitRecord()
    {
      _writer.CommitRow();
      if (!IsHeaderSerialized)
      {
        _writer.Write(_secondaryLogger);
        _writer.CommitRow();
        
        IsHeaderSerialized = true;
        _secondaryLogger.Dispose();
      }
    }

    public void WriteValue(
      NamespaceWithName? prefix,
      in ReadOnlySpan<char> fieldName, 
      in ReadOnlySpan<char> value)
    {
      if(IsHeaderSerialized || prefix is null)
      {
        _writer.WriteValue(in value);
      }
      else
      {
        string prefixValue = prefix.FullName; 
        if(prefixValue.Length > 0)
        {
          _writer.WriteValue(prefixValue);
          if(fieldName.Length > 0)
          {
            _writer.WriteValueAppend(prefix.NameSeparator);
            _writer.WriteValueAppend(in fieldName);
          }
        }
        else
        {
          _writer.WriteValue(in fieldName);
        }
        _secondaryWriter.WriteValue(in value);
      }
    }
    
    public void WriteNull(
      NamespaceWithName? prefix,
      in ReadOnlySpan<char> fieldName)
    {
      WriteValue(prefix, in fieldName, String.Empty); 
    }
  }
}