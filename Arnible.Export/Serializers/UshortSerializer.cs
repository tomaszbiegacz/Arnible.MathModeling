namespace Arnible.Export.Serializers
{
  class UshortSerializer : ValueRecordSerializerSimple<ushort>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in ushort record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}