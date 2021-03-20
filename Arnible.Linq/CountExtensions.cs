using System;

namespace Arnible.Linq
{
  public static class CountExtensions
  {
    public static ushort Count<T>(in this ReadOnlySpan<T> src, FuncIn<T, bool> func)
    {
      ushort count = 0;
      foreach(ref readonly T item in src)
      {
        if(func(in item))
        {
          count++;
        }
      }
      return count;
    }
    
    public static ushort Count<T>(in this Span<T> src, FuncIn<T, bool> func)
    {
      ushort count = 0;
      foreach(ref readonly T item in src)
      {
        if(func(in item))
        {
          count++;
        }
      }
      return count;
    }
  }
}