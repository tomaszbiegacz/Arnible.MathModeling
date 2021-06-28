namespace Arnible.Export.Test
{
  class ReadOnlyArraySerializer<T> : ValueRecordSerializerSimple<ReadOnlyArray<T>>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in ReadOnlyArray<T> record)
    {
      serializer.CollectionField<T>().Write(string.Empty, record.Span);
    }
  }
}