using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public static class IMathModelingLoggerExtensions
  {
    public static IRecordSerializerStream<T> CreateTsvNotepad<T>(this IMathModelingLogger logger, string name) where T : struct
    {
      RecordSerializerFileStream<T> result = TsvSerializer<T>.ToTempFile();
      logger.Log($"Notepad {name}: {result.Destination}");
      return result;
    }

    public static async Task LogDataSet<T>(
      this IMathModelingLogger logger,
      string name, IEnumerable<T> records,
      CancellationToken cancellationToken = default) where T : struct
    {
      await using RecordSerializerFileStream<T> result = TsvSerializer<T>.ToTempFile();
      logger.Log($"Data set {name}: {result.Destination}");
      foreach (T record in records)
      {
        await result.Serialize(record, cancellationToken);
      }
    }
  }
}
