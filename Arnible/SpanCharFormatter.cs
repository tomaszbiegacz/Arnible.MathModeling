using System;

namespace Arnible
{
  public static class SpanCharFormatter
  {
    public const ushort BufferSize = 35;
    
    public static Span<char> ToString(int value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(uint value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(in long value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(in ulong value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(float value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(in double value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
    
    public static Span<char> ToString(in decimal value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer.Slice(0, charsWritten);
    }
  }
}