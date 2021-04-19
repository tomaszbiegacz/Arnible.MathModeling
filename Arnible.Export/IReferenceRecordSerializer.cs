namespace Arnible.Export
{
  public interface IReferenceRecordSerializer<TRecord> where TRecord: class?
  {
    /// <summary>
    /// Describes procedure for serializing record
    /// </summary>
    void Serialize(IRecordFieldSerializer serializer, TRecord record);
  }
}