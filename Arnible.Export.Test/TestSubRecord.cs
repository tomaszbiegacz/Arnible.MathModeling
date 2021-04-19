namespace Arnible.Export.Test
{
  public readonly struct TestSubRecord
  {
    public TestSubRecord(int value)
    {
      Value = value;
      HiddenValue = value;
    }

    public int Value { get; }
    
    public int HiddenValue { get; }
    
    //
    // Serialization
    //
    
    public class Serializer : ValueRecordSerializerSimple<TestSubRecord>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in TestSubRecord? record)
      {
        serializer.Write(nameof(Value), record?.Value);
      }
    }
  }
}
