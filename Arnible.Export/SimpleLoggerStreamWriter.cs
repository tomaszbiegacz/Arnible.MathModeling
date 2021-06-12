using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Arnible.Logger;

namespace Arnible.Export
{
  class SimpleLoggerStreamWriter : ISimpleLogger, IAsyncDisposable
  {
    private readonly SimpleLoggerMemoryWriter _logsBuffer;
    private readonly Stream _writer;

    public SimpleLoggerStreamWriter(Stream writer)
    {
      _writer = writer;
      _logsBuffer = new SimpleLoggerMemoryWriter();
      IsLoggerEnabled = true;
    }

    public async Task Flush(CancellationToken cancellationToken)
    {
      await _logsBuffer.Flush(_writer, cancellationToken);
      await _writer.FlushAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
      await Flush(default);
      _logsBuffer.Dispose();
      await _writer.DisposeAsync();
    }

    public bool IsLoggerEnabled
    {
      get => _logsBuffer.IsLoggerEnabled; 
      set => _logsBuffer.IsLoggerEnabled = value;
    }

    public ISimpleLogger Write(in ReadOnlySpan<char> message) => _logsBuffer.Write(in message);
    
    public ISimpleLogger Write(MemoryStream message) => _logsBuffer.Write(message);
  }
}