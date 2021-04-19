using System;

namespace Arnible.Export
{
  public class ReadOnlyArraySerializer<T> : ValueRecordSerializerSimple<ReadOnlyArray<T>> where T: IEquatable<T>
  {
    static readonly Type _tType = typeof(T);
    
    public override void Serialize(IRecordFieldSerializer serializer, in ReadOnlyArray<T>? record)
    {
      if(record is null)
      {
        serializer.WriteNull(string.Empty);
      }
      else
      {
        ReadOnlySpan<T> items = record.Value;
        serializer.CollectionField<T>().Write(string.Empty, in items);
      }
    }
  }
}