namespace Arnible.Export.Serializers
{
  class DecimalSerializer : ValueRecordSerializerSimple<decimal>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in decimal record)
    {
      serializer.Write(string.Empty, in record);
    }
  }
}