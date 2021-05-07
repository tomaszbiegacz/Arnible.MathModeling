using System;
using Arnible.Logger;

namespace Arnible.Export
{
  class ValuesRowWriter
  {
    private readonly ISimpleLogger _writer;
    private readonly string _valueSeparator;

    private bool _isEmptyRow;

    public ValuesRowWriter(
      ISimpleLogger writer, 
      string valueSeparator)
    {
      _writer = writer;
      _valueSeparator =  valueSeparator;

      _isEmptyRow = true;
    }

    public void WriteValueAppend(in ReadOnlySpan<char> value)
    {
      _writer.Write(in value);
      _isEmptyRow = false;
    }
    
    public void WriteValue(in ReadOnlySpan<char> value)
    {
      if (!_isEmptyRow)
      {
        _writer.Write(_valueSeparator.AsSpan());
      }
      
      _writer.Write(in value);
      _isEmptyRow = false;
    }

    public void CommitRow()
    {
      if(!_isEmptyRow)
      {
        _writer.Write(Environment.NewLine.AsSpan());
        _isEmptyRow = true;
      }
    }
    
    public void Write(SimpleLoggerMemoryWriter logger)
    {
      if(!_isEmptyRow)
      {
        throw new InvalidOperationException("Commit row first.");
      }
      logger.Flush(_writer);
      _isEmptyRow = false;
    }
  }
}