using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public interface IRecordSerializer<T> where T : struct
  {
    SerializationMediaType MediaType { get; }

    ValueTask SerializeHeader(Stream output, CancellationToken cancellationToken);

    ValueTask SerializeRecord(T record, Stream output, CancellationToken cancellationToken);
  }
}
