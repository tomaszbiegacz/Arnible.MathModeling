namespace Arnible.Export.Serializers
{
  class UlongSerializer : ValueRecordSerializerSimple<ulong>
  {
    public override void Serialize(IRecordFieldSerializer serializer, in ulong record)
    {
      serializer.Write(string.Empty, in record);
    }
  }
}