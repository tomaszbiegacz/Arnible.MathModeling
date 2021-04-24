namespace Arnible.Export
{
  public interface IValueRecordSerializer<TRecord> where TRecord: struct
  {
    /// <summary>
    /// Describes procedure for serializing record
    /// </summary>
    void Serialize(IRecordFieldSerializer serializer, in TRecord record);
    
    /// <summary>
    /// Describes procedure for serializing record
    /// </summary>
    void Serialize(IRecordFieldSerializer serializer, in TRecord? record);
  }
}