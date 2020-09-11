namespace Arnible.MathModeling.Export.Test
{
  public class NullableSubRecord
  {
    public int NotPresentValue { get; set; } = 2;

    public int NotPresentOther { get; set; } = 3;
    
    //
    // Serialization
    //
    
    class Serializer : IRecordWriter<NullableSubRecord>
    {
      private readonly IRecordFieldSerializer _serializer;
      
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
      }
      
      public void Write(in NullableSubRecord record)
      {
        _serializer.Write(nameof(NotPresentValue), record.NotPresentValue);
        _serializer.Write(nameof(NotPresentOther), record.NotPresentOther);
        _serializer.CommitWrite();
      }

      public void WriteNull()
      {
        _serializer.WriteNull(nameof(NotPresentValue));
        _serializer.WriteNull(nameof(NotPresentOther));
        _serializer.CommitWrite();
      }
    }
    
    public static IRecordWriter<NullableSubRecord> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }
  }
}
