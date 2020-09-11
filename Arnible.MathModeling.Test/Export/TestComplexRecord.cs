namespace Arnible.MathModeling.Export.Test
{
  public struct TestComplexRecord
  {
    public int RootValue { get; set; }

    public TestSubRecord Record { get; set; }

    public NullableSubRecord? Nullable { get; set; }

    public int OtherValue { get; set; }
    
    //
    // Serialization
    //
    
    class Serializer : IRecordWriter<TestComplexRecord>
    {
      private readonly IRecordFieldSerializer _serializer;
      private readonly IRecordWriter<TestSubRecord> _serializerRecord;
      private readonly IRecordWriter<NullableSubRecord> _serializerNullable;
      
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
        _serializerRecord = serializer.GetRecordSerializer(nameof(Record), TestSubRecord.CreateSerializer);
        _serializerNullable = serializer.GetRecordSerializer(nameof(Nullable), NullableSubRecord.CreateSerializer);
      }
      
      public void Write(in TestComplexRecord record)
      {
        _serializer.Write(nameof(RootValue), record.RootValue);
        _serializerRecord.Write(record.Record);
        if (record.Nullable != null)
        {
          _serializerNullable.Write(record.Nullable);
        }
        else
        {
          _serializerNullable.WriteNull();
        }
        _serializer.Write(nameof(OtherValue), record.OtherValue);
        _serializer.CommitWrite();
      }

      public void WriteNull()
      {
        _serializer.WriteNull(nameof(RootValue));
        _serializerRecord.WriteNull();
        _serializerNullable.WriteNull();
        _serializer.WriteNull(nameof(OtherValue));
        _serializer.CommitWrite();
      }
    }
    
    public static IRecordWriter<TestComplexRecord> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }
  }
}
