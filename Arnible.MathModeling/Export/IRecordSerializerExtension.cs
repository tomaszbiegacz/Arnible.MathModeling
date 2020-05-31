using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public static class IRecordSerializerExtension
  {
    public static async Task Serialize<T>(
      this IRecordSerializer<T> serializer,
      IEnumerable<T> records,
      Stream output,
      CancellationToken cancellationToken) where T : struct
    {
      var recordSerializer = new RecordSerializerStream<T>(output, serializer);
      foreach (T record in records)
      {
        await recordSerializer.Serialize(record, cancellationToken);
      }
    }
  }
}
