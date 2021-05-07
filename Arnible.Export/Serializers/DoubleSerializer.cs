namespace Arnible.Export.Serializers
{
  class DoubleSerializer : ValueRecordSerializerSimple<double>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in double record)
    {
      serializer.Write(string.Empty, in record);
    }
  }
}