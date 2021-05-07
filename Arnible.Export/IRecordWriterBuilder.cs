namespace Arnible.Export
{
  public interface IRecordWriterBuilder
  {
    IReferenceRecordWriter<TRecord> CreateTsvReferenceRecordWriter<TRecord>(
      ISimpleLogger logger) where TRecord : class;
    
    IValueRecordWriter<TRecord> CreateTsvValueRecordWriter<TRecord>(
      ISimpleLogger logger) where TRecord : struct;
    
    IRecordWriter CreateTsvRecordWriter(ISimpleLogger logger);
  }
}