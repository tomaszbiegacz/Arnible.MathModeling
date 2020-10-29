using Arnible.MathModeling.Export;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Arnible.MathModeling.xunit
{
  public sealed class XunitLogger : IMathModelingLogger, ILogger, IDisposable, IAsyncDisposable
  {
    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _stringBuffer;
    private readonly FileInfo _logFile;

    public XunitLogger(ITestOutputHelper output)
    {
      _output = output;
      _stringBuffer = new StringBuilder();
      _logFile = new FileInfo(Path.GetTempFileName());
      
      _output.WriteLine($"Log file: {_logFile.FullName}");
    }

    //
    // IMathModelingLogger
    //

    public void Log(in string message)
    {
      _stringBuffer.AppendLine(message);
    }

    //
    // ILogger
    //

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      // ReSharper disable once HeapView.PossibleBoxingAllocation
      Log(state?.ToString() ?? string.Empty);
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => this;

    //
    // IDisposable
    //

    public void Dispose()
    {
      // do nothing
    }

    public async ValueTask DisposeAsync()
    {
      const int maxLength = 9000;
      string logs = _stringBuffer.ToString();
      if (logs.Length > maxLength)
      {
        _output.WriteLine(logs.Substring(0, maxLength));
      }
      else
      {
        _output.WriteLine(logs);  
      }
      await File.WriteAllTextAsync(_logFile.FullName, logs);
    }
  }
}
