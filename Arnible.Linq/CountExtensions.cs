using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class CountExtensions
  {
    public static uint Count<T>(this IEnumerable<T> source)
    {
      using IEnumerator<T> e = source.GetEnumerator();
      
      uint count = 0;
      while (e.MoveNext())
      {
        count++;
      }
      return count;
    }
    
    public static uint Count<T>(this IEnumerable<T> source, Func<T, bool> func)
    {
      uint count = 0;
      foreach(T item in source)
      {
        if(func(item))
        {
          count++;
        }
      }
      return count;
    }
    
    public static ushort Count<T>(this IReadOnlyList<T> source)
    {
      return (ushort)source.Count;
    }
    
    public static ushort Count<T>(this IList<T> source)
    {
      return (ushort)source.Count;
    }
    
    public static ushort Count<T>(in this ReadOnlySpan<T> src)
    {
      return (ushort)src.Length;
    }
    
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
    
    public static ushort Count<T>(in this Span<T> src)
    {
      return (ushort)src.Length;
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