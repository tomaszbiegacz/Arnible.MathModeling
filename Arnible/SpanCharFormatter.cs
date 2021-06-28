using System;
using System.Globalization;

namespace Arnible
{
  public static class SpanCharFormatter
  {
    public const ushort BufferSize = 35;
    
    public static ReadOnlySpan<char> ToString(int value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(uint value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(in long value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(in ulong value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(float value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(in double value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
    
    public static ReadOnlySpan<char> ToString(in decimal value, in Span<char> buffer)
    {
      if(!value.TryFormat(buffer, out int charsWritten, provider: NumberFormatInfo.InvariantInfo))
      {
        throw new ArgumentException(nameof(buffer));
      }
      return buffer[..charsWritten];
    }
  }
}