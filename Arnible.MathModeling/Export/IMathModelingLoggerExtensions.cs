using System;

namespace Arnible.MathModeling.Export
{
  public static class IMathModelingLoggerExtensions
  {
    public static IRecordWriterDisposable<T> CreateTsvNotepad<T>(
      this IMathModelingLogger logger, 
      in string name,
      Func<IRecordFieldSerializer, IRecordWriter<T>> writerFactory)
    {
      TsvFileSerializer serializer = new TsvFileSerializer();
      IRecordWriter<T> writer = writerFactory(serializer.FieldSerializer);
      
      logger.Log($"Notepad {name}: {serializer.Destination}");
      return new RecordWriterDisposable<T>(serializer, writer);
    }
  }
}
