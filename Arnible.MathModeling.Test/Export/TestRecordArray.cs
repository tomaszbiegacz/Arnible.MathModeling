namespace Arnible.MathModeling.Export.Test
{
  public struct TestRecordArray
  {
    public NullableSubRecord[]? Records { get; set; }
    
    //
    // Serialization
    //
    
    class Serializer : IRecordWriter<TestRecordArray>
    {
      private readonly IRecordFieldSerializer _serializer;
      private readonly IRecordWriterReadOnlyCollection<NullableSubRecord> _serializerNullable;
      
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
        _serializerNullable = serializer.GetReadOnlyCollectionSerializer(nameof(Records), NullableSubRecord.CreateSerializer);
      }
      
      public void Write(in TestRecordArray record)
      {
        _serializerNullable.Write(record.Records);
        _serializer.CommitWrite();
      }

      public void WriteNull()
      {
        _serializerNullable.Write(null);
        _serializer.CommitWrite();
      }
    }
    
    public static IRecordWriter<TestRecordArray> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }
  }
}
