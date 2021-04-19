using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowValueCollectionFieldSerializer<TField> : ICollectionFieldSerializer<TField> where TField: struct
  {
    private readonly RecordPerRowFieldSerializer _parent;
    
    public RecordPerRowValueCollectionFieldSerializer(RecordPerRowFieldSerializer parent)
    {
      _parent = parent;
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in ReadOnlySpan<TField> field)
    {
      _parent.GetValueRecordCollectionWriter<TField>(in fieldName).Write(in field);
    }

    public void Write(in ReadOnlySpan<char> fieldName, TField[]? field)
    {
      _parent.GetValueRecordCollectionWriter<TField>(in fieldName).Write(field);
    }

    public void Write(in ReadOnlySpan<char> fieldName, IReadOnlyCollection<TField>? field)
    {
      _parent.GetValueRecordCollectionWriter<TField>(in fieldName).Write(field);
    }
  }
}