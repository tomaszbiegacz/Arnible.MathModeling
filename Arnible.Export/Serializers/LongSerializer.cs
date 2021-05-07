namespace Arnible.Export.Serializers
{
  class LongSerializer : ValueRecordSerializerSimple<long>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in long record)
    {
      serializer.Write(string.Empty, in record);
    }
  }
}