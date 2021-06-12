using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.Logger
{
  /// <summary>
  /// Simple logger writing logs into memory
  /// </summary>
  public class SimpleLoggerMemoryWriter : ISimpleLogger, IDisposable
  {
    private readonly MemoryStream _writer;

    public SimpleLoggerMemoryWriter()
    {
      _writer = new MemoryStream();
      IsLoggerEnabled = true;
    }

    public void Dispose()
    {
      _writer.Dispose();
    }
    
    public bool IsLoggerEnabled { get; set; }

    public ISimpleLogger Write(in ReadOnlySpan<char> message)
    {
      if (IsLoggerEnabled)
      {
        Span<byte> buffer = stackalloc byte[message.Length * 2];
        OperationStatus status = Utf8.FromUtf16(message, buffer, out int _, out int bytesWritten);
        if(status != OperationStatus.Done)
        {
          throw new InvalidOperationException($"Got status {status} when writing down [{message.ToString()}]");
        }
        _writer.Write(buffer[..bytesWritten]);
      }
      return this;
    }
    
    public ISimpleLogger Write(MemoryStream message)
    {
      if (IsLoggerEnabled)
      {
        message.WriteTo(_writer);
      }
      return this;
    }
    
    public async Task Flush(Stream output, CancellationToken cancellationToken)
    {
      _writer.Position = 0;
      await _writer.CopyToAsync(output, 1024, cancellationToken);

      // reset buffer position
      _writer.Position = 0;
    }

    public void Flush(ISimpleLogger output)
    {
      output.Write(_writer);
      
      // reset buffer position
      _writer.Position = 0;
    }
    
    public void Flush(out string output)
    {
      output = Encoding.UTF8.GetString(_writer.ToArray());
      
      // reset buffer position
      _writer.Position = 0;
    }
  }
}