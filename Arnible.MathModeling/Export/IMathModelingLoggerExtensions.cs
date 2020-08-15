namespace Arnible.MathModeling.Export
{
  public static class IMathModelingLoggerExtensions
  {
    public static IRecordSerializerStream<T> CreateTsvNotepad<T>(
      this IMathModelingLogger logger, 
      in string name) where T : struct
    {
      RecordSerializerFileStream<T> result = TsvSerializer<T>.ToTempFile();
      logger.Log($"Notepad {name}: {result.Destination}");
      return result;
    }
  }
}
