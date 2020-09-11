using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  class RecordPerRowSerializerReadOnlyCollection<T> : RecordPerRowSerializerSubfield, IRecordWriterReadOnlyCollection<T>
  {
    private readonly string _headerPartsSeparator;
    private readonly string _fieldNamePrefix;
    private readonly IRecordWriter<T> _recordSerializer;
    private uint _position;

    public RecordPerRowSerializerReadOnlyCollection(
      in RecordPerRowSerializerField fieldSerializer,
      in string fieldName,
      in Func<IRecordFieldSerializer, IRecordWriter<T>> writerFactory)
      : base(fieldSerializer)
    {
      if (fieldName.Length > 0)
      {
        _fieldNamePrefix = fieldName + fieldSerializer.HeaderPartsSeparator;
      }
      else
      {
        _fieldNamePrefix = string.Empty;
      }

      _headerPartsSeparator = fieldSerializer.HeaderPartsSeparator.ToString();
      _recordSerializer = writerFactory(this);
    }

    public void Write(in IReadOnlyCollection<T>? records)
    {
      if (records != null)
      {
        _position = 0;
        foreach (T record in records)
        {
          _recordSerializer.Write(in record);
          _position++;
        }
      }
    }

    protected override string GetFieldName(in string fieldName)
    {
      if (IsSerializingFieldName)
      {
        if (fieldName.Length > 0)
        {
          return $"{_fieldNamePrefix}{_position.ToString()}{_headerPartsSeparator}{fieldName}";  
        }
        else
        {
          return $"{_fieldNamePrefix}{_position.ToString()}";
        }
      }
      else
      {
        return string.Empty;
      }
    }
  }
}