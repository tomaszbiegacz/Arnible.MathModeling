using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace Arnible.Xunit
{
  public sealed class XunitLogger : ISimpleLogger, ILogger, IDisposable
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
    public void Write(in ReadOnlySpan<char> message)
    {
      if(IsLoggerEnabled)
      {
        _stringBuffer.Append(message);
      }
    }

    public void Write(MemoryStream message)
    {
      if(IsLoggerEnabled)
      {
        StreamReader reader = new StreamReader(message);
        _stringBuffer.Append(reader.ReadToEnd());
      }
    }

    public bool SaveLogsToFile { get; set; }

    //
    // ILogger
    //

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      if (IsLoggerEnabled)
      {
        Write(formatter(state, exception));
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
