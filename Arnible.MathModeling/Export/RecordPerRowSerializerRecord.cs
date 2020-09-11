using System;

namespace Arnible.MathModeling.Export
{
  class RecordPerRowSerializerRecord<T> : RecordPerRowSerializerSubfield, IRecordWriter<T>
  {
    private readonly string _fieldName;
    private readonly string _fieldNamePrefix;
    private readonly IRecordWriter<T> _recordSerializer;

    public RecordPerRowSerializerRecord(
      in RecordPerRowSerializerField fieldSerializer,
      in string fieldName,
      in Func<IRecordFieldSerializer, IRecordWriter<T>> writerFactory)
    : base(fieldSerializer)
    {
      _fieldName = fieldName;
      _fieldNamePrefix = fieldName + fieldSerializer.HeaderPartsSeparator;
      _recordSerializer = writerFactory(this);
    }

    public void Write(in T record)
    {
      _recordSerializer.Write(in record);
    }

    public void WriteNull()
    {
      _recordSerializer.WriteNull();
    }

    protected override string GetFieldName(in string fieldName)
    {
      if (IsSerializingFieldName)
      {
        if (fieldName.Length > 0)
        {
          return _fieldNamePrefix + fieldName;  
        }
        else
        {
          return _fieldName;
        }
      }
      else
      {
        return string.Empty;
      }
    }
  }
}