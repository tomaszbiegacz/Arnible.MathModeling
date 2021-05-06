using System;

namespace Arnible.Export.Serializers
{
  class ReadOnlyArraySerializer<T> : ValueRecordSerializerSimple<ReadOnlyArray<T>> where T: IEquatable<T>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in ReadOnlyArray<T> record)
    {
      serializer.CollectionField<T>().Write(string.Empty, record);
    }
  }
}