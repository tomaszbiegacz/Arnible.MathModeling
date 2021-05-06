namespace Arnible.Export.Serializers
{
  class ShortSerializer : ValueRecordSerializerSimple<short>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in short record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}