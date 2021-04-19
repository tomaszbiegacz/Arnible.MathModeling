using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowReferenceCollectionFieldWriter<TField> : 
    RecordPerRowFieldSerializer, 
    IRecordCollectionWriter<TField>
    where TField: class?
  {
    private readonly IReferenceRecordSerializer<TField> _serializer;

    public RecordPerRowReferenceCollectionFieldWriter(
      RecordPerRowFieldSerializer parent,
      string fieldName,
      IReferenceRecordSerializer<TField> serializer) : base(parent, fieldName)
    {
      _serializer = serializer;
    }
    
    public void Write(in ReadOnlySpan<TField> records)
    {
      RecordPerRowCollectionItemFieldSerializer fieldSerializer = new(this);
      foreach(ref readonly TField record in records)
      {
        fieldSerializer.BeginRecord();
        _serializer.Serialize(fieldSerializer, record);
        fieldSerializer.Position += 1;
      }
    }
    
    public void Write(TField[]? records)
    {
      if(records is not null)
      {
        RecordPerRowCollectionItemFieldSerializer fieldSerializer = new(this);
        for(ushort i=0; i<records.Length; ++i)
        {
          ref readonly TField record = ref records[i];
          
          fieldSerializer.BeginRecord();
          _serializer.Serialize(fieldSerializer, record);
          fieldSerializer.Position += 1;
        }
      }
    }

    public void Write(IReadOnlyCollection<TField>? records)
    {
      if(records is not null)
      {
        RecordPerRowCollectionItemFieldSerializer fieldSerializer = new(this);
        foreach(TField record in records)
        {
          fieldSerializer.BeginRecord();
          _serializer.Serialize(fieldSerializer, record);
          fieldSerializer.Position += 1;
        }
      }
    }
  }
}