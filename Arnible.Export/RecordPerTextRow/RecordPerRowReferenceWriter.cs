using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowReferenceWriter<TRecord> : 
    RecordPerRowFieldSerializer, 
    IReferenceRecordWriter<TRecord> 
    where TRecord : class?
  {
    private readonly IReferenceRecordSerializer<TRecord> _serializer;
    
    public RecordPerRowReferenceWriter(
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
      _serializer = GetReferenceSerializer<TRecord>();
    }

    public void Write(TRecord record)
    {
      BeginRecord();
      _serializer.Serialize(this, record);
      CommitStream();
    }
  }
}