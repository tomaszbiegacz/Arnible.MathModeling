namespace Arnible.Export.Test
{
  public class TestSubReferenceRecord
  {
    public TestSubReferenceRecord(int value)
    {
      Value = value;
      HiddenValue = value;
    }

    public int Value { get; }
    
    public int HiddenValue { get; }
    
    //
    // Serialization
    //
    
    public class Serializer : IReferenceRecordSerializer<TestSubReferenceRecord>
    {
      public void Serialize(IRecordFieldSerializer serializer, TestSubReferenceRecord? record)
      {
        serializer.Write(nameof(Value), record?.Value);
      }
    }
  }
}