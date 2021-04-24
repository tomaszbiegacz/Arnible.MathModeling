using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowValueWriter<TRecord> : 
    RecordPerRowFieldSerializer, 
    IValueRecordWriter<TRecord> 
    where TRecord : struct
  {
    private readonly IValueRecordSerializer<TRecord> _serializer;
    
    public RecordPerRowValueWriter(
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
      _serializer = GetValueSerializer<TRecord>();
    }

    public void Write(in TRecord? record)
    {
      BeginRecord();
      _serializer.Serialize(this, in record);
      CommitStream();
    }
    
    public void Write(in TRecord record)
    {
      BeginRecord();
      _serializer.Serialize(this, in record);
      CommitStream();
    }
  }
}