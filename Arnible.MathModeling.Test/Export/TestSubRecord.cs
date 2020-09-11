namespace Arnible.MathModeling.Export.Test
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
    
    class Serializer : IRecordWriter<TestSubRecord>
    {
      private readonly IRecordFieldSerializer _serializer;
      
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
      }
      
      public void Write(in TestSubRecord record)
      {
        _serializer.Write(nameof(Value), record.Value);
        _serializer.CommitWrite();
      }

      public void WriteNull()
      {
        _serializer.WriteNull(nameof(Value));
        _serializer.CommitWrite();
      }
    }
    
    public static IRecordWriter<TestSubRecord> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }
  }
}
