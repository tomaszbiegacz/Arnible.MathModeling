namespace Arnible.Export.Test
{
  public readonly struct TestSubValueRecord
  {
    public TestSubValueRecord(int value)
    {
      Value = value;
      HiddenValue = value;
    }

    public int Value { get; }
    
    public int HiddenValue { get; }
    
    //
    // Serialization
    //
    
    public class Serializer : IValueRecordSerializer<TestSubValueRecord>
    {
      public void Serialize(IRecordFieldSerializer serializer, in TestSubValueRecord record)
      {
        serializer.Write(nameof(Value), record.Value);
      }
      
      public void Serialize(IRecordFieldSerializer serializer, in TestSubValueRecord? record)
      {
        serializer.Write(nameof(Value), record?.Value);
      }
    }
  }
}
