using System;

namespace Arnible.Linq
{
  public static class ToCharMemoryExtensions
  {
    public static ReadOnlyMemory<char> ToCharMemory<T>(this in ReadOnlySpan<T> src, char separator = ',')
    {
      return ("[" + string.Join(separator, src.ToArray()) + "]").AsMemory();
    }
  }
}