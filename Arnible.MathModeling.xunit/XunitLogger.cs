using Arnible.MathModeling.Export;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace Arnible.MathModeling.xunit
{
  public sealed class XunitLogger : IMathModelingLogger, ILogger, IDisposable
  {
    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _stringBuffer;

    public XunitLogger(ITestOutputHelper output)
    {
      _output = output;
      _stringBuffer = new StringBuilder();

      IsLoggerEnabled = true;
      SaveLogsToFile = false;
    }

    public bool IsLoggerEnabled { get; set; }
    public bool SaveLogsToFile { get; set; }

    //
    // IMathModelingLogger
    //

    public void Log(string message)
    {
      if (IsLoggerEnabled)
      {
        _stringBuffer.AppendLine(message);  
      }
    }

    //
    // ILogger
    //

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      if (IsLoggerEnabled)
      {
        Log(formatter(state, exception));
      }
    }

    public bool IsEnabled(LogLevel logLevel) => IsLoggerEnabled;

    public IDisposable BeginScope<TState>(TState state) => this;

    //
    // IDisposable
    //

    /// <summary>
    /// Write logs to ITestOutputHelper and to file
    /// </summary>
    public void Dispose()
    {
      if (IsLoggerEnabled)
      {
        string logs = _stringBuffer.ToString();
        _stringBuffer.Clear();

        FileInfo? logFile = null;
        if (SaveLogsToFile)
        {
          logFile = new FileInfo(Path.GetTempFileName());
          _output.WriteLine($"Log file: {logFile.FullName}");
        }
      
        const int maxLength = 9000;
        if (logs.Length > maxLength)
        {
          _output.WriteLine(logs.Substring(0, maxLength));
        }
        else
        {
          _output.WriteLine(logs);  
        }

        if (logFile != null)
        {
          File.WriteAllText(logFile.FullName, logs);
        } 
      }
    }
  }
}
