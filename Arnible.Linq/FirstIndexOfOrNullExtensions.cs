using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class FirstIndexOfOrNullExtensions
  {
    public static ushort? FirstIndexOfOrNull<T>(this IReadOnlyList<T> src, Func<T, bool> predicate)
    {      
      for(ushort i=0; i<src.Count; ++i)
      {
        if (predicate(src[i]))
        {
          return i;
        }
      }

      return null;
    }    
    
    public static ushort? FirstIndexOfOrNull<T>(this IReadOnlyList<T> src, in T item) where T : IEquatable<T>
    {      
      for(ushort i=0; i<src.Count; ++i)
      {
        if (item.Equals(src[i]))
        {
          return i;
        }
      }

      return null;
    }    
  }
}