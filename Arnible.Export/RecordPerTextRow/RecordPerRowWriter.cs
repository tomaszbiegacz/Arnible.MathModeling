using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowWriter :
    RecordPerRowFieldSerializer,
    IRecordWriter
  {
    class RecordSerializer : IRecordSerializer
    {
      private readonly RecordPerRowWriter _writer;
      private bool _isDisposed;
      
      public RecordSerializer(RecordPerRowWriter writer)
      {
        _writer = writer;
        _isDisposed = true;
      }
      
      public IRecordSerializer BeginRecord()
      {
        if (!_isDisposed)
        {
          throw new InvalidOperationException("Serializer is not ready");
        }
        
        _writer.BeginRecord();
        _isDisposed = false;
        return this;
      }

      public void Dispose()
      {
        if (!_isDisposed)
        {
          _writer.CommitStream();
          _isDisposed = true;
        }
      }

      public IRecordFieldSerializer FieldSerializer
      {
        get
        {
          if (_isDisposed)
          {
            throw new InvalidOperationException("Serializer is already disposed");
          }
          return _writer;
        }
      }
    }
    
    private readonly RecordSerializer _serializer;
    
    public RecordPerRowWriter(
      string headerFieldSubNameSeparator,
      string rowFieldSeparator,
      IReadOnlyDictionary<Type, Func<Type[], object>> serializersFactories,
      ISimpleLogger logger) 
      : base(
        headerFieldSubNameSeparator: headerFieldSubNameSeparator, 
        rowFieldSeparator: rowFieldSeparator, 
        serializersFactories: serializersFactories, 
        logger: logger)
    {
      _serializer = new RecordSerializer(this);
    }

    public IRecordSerializer OpenRecord()
    {
      return _serializer.BeginRecord();
    }
  }
}