namespace Arnible.Export.Serializers
{
  class UintSerializer : ValueRecordSerializerSimple<uint>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in uint record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}