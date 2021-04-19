namespace Arnible.Export.Test
{
  public record NullableSubRecord
  {
    public int NotPresentValue { get; set; } = 2;

    public int NotPresentOther { get; set; } = 3;
    
    //
    // Serialization
    //
    
    public class Serializer : IReferenceRecordSerializer<NullableSubRecord>
    {
      public void Serialize(IRecordFieldSerializer serializer, NullableSubRecord? record)
      {
        serializer.Write(nameof(NotPresentValue), record?.NotPresentValue);
        serializer.Write(nameof(NotPresentOther), record?.NotPresentOther);
      }
    }
  }
}
