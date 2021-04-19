namespace Arnible.Export.Test
{
  public struct ValueRecord
  {
    public int RootValue { get; set; }

    public TestSubRecord Record { get; set; }

    public NullableSubRecord? Nullable { get; set; }

    public int OtherValue { get; set; }
    
    //
    // Serialization
    //
    
    public class Serializer : ValueRecordSerializerSimple<ValueRecord>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in ValueRecord? record)
      {
        serializer.Write(nameof(RootValue), record?.RootValue);
        serializer.WriteValueField(nameof(Record), record?.Record);
        serializer.WriteReferenceField(nameof(Nullable), record?.Nullable);
        serializer.Write(nameof(OtherValue), record?.OtherValue);
      }
    }
  }
}
