namespace Arnible.Export.Serializers
{
  class SbyteSerializer : ValueRecordSerializerSimple<sbyte>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in sbyte record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}