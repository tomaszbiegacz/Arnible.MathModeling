using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class AllExtensions
  {
    public static void AssertAll<T>(this IEnumerable<T> src, Func<T, bool> predicate)
    {
      ushort i=0;
      foreach(T item in src)
      {
        if(!predicate(item))
        {
          throw new AssertException(
            $"At position {i} item [{item}] doesn't meet the condition."
          );
        }
        i++;
      }
    }
    
    public static void AssertAll<T>(in this ReadOnlySpan<T> src, FuncIn<T, bool> predicate)
    {
      for(ushort i=0; i<src.Length; ++i)
      {
        if(!predicate(in src[i]))
        {
          throw new AssertException(
            $"At position {i} item [{src[i]}] doesn't meet the condition.", 
            AssertException.ToString(src)
          );
        }
      }
    }
    
    public static void AssertAll<T>(in this Span<T> src, FuncIn<T, bool> predicate)
    {
      AssertAll((ReadOnlySpan<T>)src, predicate);
    }
  }
}