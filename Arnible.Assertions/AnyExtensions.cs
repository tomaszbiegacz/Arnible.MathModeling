using System;
using System.Collections.Generic;

namespace Arnible.Assertions
{
  public static class AnyExtensions
  {
    public static void AssertAny<T>(this IEnumerable<T> src, Func<T, bool> predicate)
    {
      foreach(T item in src)
      {
        if(predicate(item))
        {
          return;
        }
      }
      throw new AssertException($"No item meets condition.");
    }
    
    public static void AssertAny<T>(in this ReadOnlySpan<T> src, FuncIn<T, bool> predicate)
    {
      for(ushort i=0; i<src.Length; ++i)
      {
        if(predicate(in src[i]))
        {
          return;
        }
      }
      throw new AssertException($"No item meets condition.", AssertException.ToString(src));
    }
    
    public static void AssertAny<T>(in this Span<T> src, FuncIn<T, bool> predicate)
    {
      AssertAny((ReadOnlySpan<T>)src, predicate);
    }
  }
}