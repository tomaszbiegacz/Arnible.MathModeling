using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowReferenceCollectionFieldSerializer<TField> : ICollectionFieldSerializer<TField> where TField: class?
  {
    private readonly RecordPerRowFieldSerializer _parent;
    
    public RecordPerRowReferenceCollectionFieldSerializer(RecordPerRowFieldSerializer parent)
    {
      _parent = parent;
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in ReadOnlySpan<TField> field)
    {
      _parent.GetReferenceRecordCollectionWriter<TField>(in fieldName).Write(in field);
    }

    public void Write(in ReadOnlySpan<char> fieldName, TField[]? field)
    {
      _parent.GetReferenceRecordCollectionWriter<TField>(in fieldName).Write(field);
    }

    public void Write(in ReadOnlySpan<char> fieldName, IReadOnlyCollection<TField>? field)
    {
      _parent.GetReferenceRecordCollectionWriter<TField>(in fieldName).Write(field);
    }
  }
}