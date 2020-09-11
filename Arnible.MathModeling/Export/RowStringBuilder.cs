using System.Text;

namespace Arnible.MathModeling.Export
{
  class RowStringBuilder
  {
    private readonly StringBuilder _buffer;
    private readonly char _fieldSeparator;
    private bool _isEmptyRow;

    public RowStringBuilder(char fieldSeparator)
    {
      _buffer = new StringBuilder();
      _fieldSeparator = fieldSeparator;
      _isEmptyRow = true;
    }
    
    public void Add(in string value)
    {
      if (!_isEmptyRow)
      {
        _buffer.Append(_fieldSeparator);
      }
      
      _buffer.Append(value);
      _isEmptyRow = false;
    }
    public void Append(in uint value) => _buffer.Append(value);
    public void Append(in char value) => _buffer.Append(value);

    public string Flush()
    {
      string result = _buffer.ToString();
      _buffer.Clear();
      _isEmptyRow = true;
      return result;
    }
  }
}