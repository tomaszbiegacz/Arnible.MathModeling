using System;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace Arnible.Xunit
{
  public sealed class XunitLogger : ISimpleLoggerForTests
  {
    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _stringBuffer;
    private FileInfo? _logFile;

    public XunitLogger(ITestOutputHelper output)
    {
      _output = output;
      _stringBuffer = new StringBuilder();
      _logFile = null;

      IsLoggerEnabled = true;
      IsSavingLogsToFileEnabled = false;
    }

    public bool IsLoggerEnabled { get; private set; }
    
    public bool IsSavingLogsToFileEnabled { get; private set; }
    
    public void EnableLogging(bool value)
    {
      IsLoggerEnabled = value;
    }
    
    public ISimpleLogger Write(in ReadOnlySpan<char> message)
    {
      if(IsLoggerEnabled)
      {
        _stringBuffer.Append(message);
      }
      return this;
    }

    public ISimpleLogger Write(MemoryStream message)
    {
      if(IsLoggerEnabled)
      {
        StreamReader reader = new StreamReader(message);
        _stringBuffer.Append(reader.ReadToEnd());
      }
      return this;
    }

    public void SaveLogsToFile(bool value)
    {
      IsSavingLogsToFileEnabled = value;
    }

    public void Flush()
    {
      string logs = _stringBuffer.ToString();
      _stringBuffer.Clear();
      
      if (IsSavingLogsToFileEnabled && _logFile == null)
      {
        _logFile = new FileInfo(Path.GetTempFileName());
        _output.WriteLine($"Log file: {_logFile.FullName}");
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

      if (_logFile != null)
      {
        File.WriteAllText(_logFile.FullName, logs);
      } 
    }

    /// <summary>
    /// Write logs to ITestOutputHelper and to file
    /// </summary>
    public void Dispose()
    {
      Flush();
    }
  }
}
