namespace Arnible.Export.Test
{
  public struct TestRecordArray
  {
    public NullableSubRecord[]? Records { get; set; }
    
    public TestRecord[]? RecordsValues { get; set; }
    
    public TestRecord[] RecordsValuesNotNull { get; set; }
    
    //
    // Serialization
    //
    
    public class Serializer : ValueRecordSerializerSimple<TestRecordArray>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in TestRecordArray? record)
      {
        serializer.CollectionField<NullableSubRecord>().Write(nameof(Records), record?.Records);
        serializer.CollectionField<TestRecord>().Write(nameof(RecordsValues), record?.RecordsValues);
        serializer.CollectionField<TestRecord>().Write(nameof(RecordsValuesNotNull), record?.RecordsValuesNotNull);
      }
    }
  }
}
