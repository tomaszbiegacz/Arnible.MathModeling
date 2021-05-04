using System.Collections.Generic;

namespace Arnible.Export.Test
{
  public struct TestRecordArray
  {
    public NullableSubRecord[]? Records { get; set; }
    
    public TestValueRecord[]? RecordsValues { get; set; }
    
    public TestValueRecord[] RecordsValuesNotNull { get; set; }
    
    public List<NullableSubRecord> RecordsValuesList { get; set; }
    
    //
    // Serialization
    //
    
    public class Serializer : ValueRecordSerializerSimple<TestRecordArray>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in TestRecordArray? record)
      {
        serializer.CollectionField<NullableSubRecord>().Write(nameof(Records), record?.Records);
        serializer.CollectionField<TestValueRecord>().Write(nameof(RecordsValues), record?.RecordsValues);
        serializer.CollectionField<TestValueRecord>().Write(nameof(RecordsValuesNotNull), record?.RecordsValuesNotNull);
        serializer.CollectionField<NullableSubRecord>().Write(nameof(RecordsValuesList), record?.RecordsValuesList);
      }
    }
  }
}
