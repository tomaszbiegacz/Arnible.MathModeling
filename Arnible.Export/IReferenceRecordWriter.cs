namespace Arnible.Export
{
  /// <summary>
  /// The simplest writer api that you can find.
  /// API is NOT thread safe.
  /// </summary>
  public interface IReferenceRecordWriter<TRecord> where TRecord: class?
  {
    /// <summary>
    /// Serialize record into memory to persisting it at some point.
    /// This is meant to be lightweight operation with O(n) complexity and minimal memory footprint. 
    /// </summary>
    void Write(TRecord record);
  }
}