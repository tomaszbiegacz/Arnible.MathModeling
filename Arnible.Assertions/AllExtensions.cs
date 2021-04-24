using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class AllExtensions
  {
    public static void AssertAll<T>(this IEnumerable<T> src, Func<T, bool> predicate)
    {
      T[] srcMaterialized = src.ToArray();
      for(ushort i=0; i<srcMaterialized.Length; ++i)
      {
        if(!predicate(srcMaterialized[i]))
        {
          throw new AssertException(
            $"At position {i} item [{srcMaterialized[i]}] doesn't meet the condition.", 
            AssertException.ToString(srcMaterialized)
            );
        }
      }
    }
  }
}