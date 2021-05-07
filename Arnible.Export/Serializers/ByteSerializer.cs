namespace Arnible.Export.Serializers
{
  class ByteSerializer : ValueRecordSerializerSimple<byte>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in byte record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}