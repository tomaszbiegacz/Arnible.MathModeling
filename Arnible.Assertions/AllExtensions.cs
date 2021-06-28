using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class AllExtensions
  {
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
    
    public static void AssertAll(in this Span<bool> src)
    {
      AssertAll((ReadOnlySpan<bool>)src);
    }
    
    public static void AssertAll(in this ReadOnlySpan<bool> src)
    {
      for(ushort i=0; i<src.Length; ++i)
      {
        if(!src[i])
        {
          throw new AssertException(
            $"At position {i} item doesn't meet the condition.", 
            AssertException.ToString(src)
          );
        }
      }
    }
  }
}