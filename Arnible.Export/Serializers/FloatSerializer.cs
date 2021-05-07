namespace Arnible.Export.Serializers
{
  class FloatSerializer : ValueRecordSerializerSimple<float>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in float record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}