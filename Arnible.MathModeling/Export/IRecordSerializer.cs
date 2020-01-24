using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public interface IRecordSerializer<T> where T : struct
  {
    SerializationMediaType MediaType { get; }

    Task SerializeHeader(Stream output, CancellationToken cancellationToken);

    Task SerializeRecord(T records, Stream output, CancellationToken cancellationToken);
  }
}
