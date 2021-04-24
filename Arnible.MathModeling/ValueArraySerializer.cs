using System;
using Arnible.Export;

namespace Arnible.MathModeling
{
  public class ValueArraySerializer<T> : ValueRecordSerializerSimple<ValueArray<T>>
    where T : struct
  {
    public override void Serialize(IRecordFieldSerializer serializer, in ValueArray<T>? record)
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