using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public interface IRecordSerializer<T>
  {
    SerializationMediaType MediaType { get; }

    Task SerializeHeader(Stream output, CancellationToken cancellationToken);

    Task SerializeRecord(T record, Stream output, CancellationToken cancellationToken);
  }
}
