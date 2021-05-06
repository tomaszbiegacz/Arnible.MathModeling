using System.Collections.Generic;

namespace Arnible.Export.Test
{
  public struct TestReferenceRecordArray
  {
    public NullableSubRecord[]? Records { get; set; }
    
    public TestValueRecord[]? RecordsValues { get; set; }
    
    public TestValueRecord[] RecordsValuesNotNull { get; set; }
    
    public List<NullableSubRecord> RecordsReferencesList { get; set; }
    
    public List<TestSubValueRecord> RecordsValuesList { get; set; }
    
    //
    // Serialization
    //
    
    public class Serializer : ValueRecordSerializerSimple<TestReferenceRecordArray>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in TestReferenceRecordArray record)
      {
        serializer.CollectionField<NullableSubRecord>().Write(nameof(Records), record.Records);
        serializer.CollectionField<TestValueRecord>().Write(nameof(RecordsValues), record.RecordsValues);
        serializer.CollectionField<TestValueRecord>().Write(nameof(RecordsValuesNotNull), record.RecordsValuesNotNull);
        serializer.CollectionField<NullableSubRecord>().Write(nameof(RecordsReferencesList), record.RecordsReferencesList);
        serializer.CollectionField<TestSubValueRecord>().Write(nameof(RecordsValuesList), record.RecordsValuesList);
      }
    }
  }
}
