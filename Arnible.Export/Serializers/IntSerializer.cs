namespace Arnible.Export.Serializers
{
  class IntSerializer : ValueRecordSerializerSimple<int>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in int record)
    {
      serializer.Write(string.Empty, record);
    }
  }
}