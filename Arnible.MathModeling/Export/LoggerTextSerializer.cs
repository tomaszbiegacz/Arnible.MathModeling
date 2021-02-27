using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  class LoggerTextSerializer : IMathModelingLogger, IAsyncDisposable
  {
    private readonly ConcurrentQueue<string> _logsBuffer;
    private readonly TextWriter _writer;

    public LoggerTextSerializer(TextWriter writer)
    {
      _writer = writer;
      _logsBuffer = new ConcurrentQueue<string>();
      IsLoggerEnabled = true;
    }

    public async Task Flush()
    {
      while (_logsBuffer.TryDequeue(out string message))
      {
        await _writer.WriteLineAsync(message.AsMemory());
      }

      await _writer.FlushAsync();
    }
    
    //
    // IAsyncDisposable
    //
    
    public async ValueTask DisposeAsync()
    {
      await Flush();
      await _writer.DisposeAsync();
    }
    
    //
    // IMathModelingLogger
    //

    public bool IsLoggerEnabled { get; }

    public void Log(string message)
    {
      if (IsLoggerEnabled)
      {
        _logsBuffer.Enqueue(message);
      }
    }
  }
}