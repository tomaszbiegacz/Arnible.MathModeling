using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SequenceEqualExtensions
  {
    public static bool SequenceEqual<TSource>(
      this IEnumerable<TSource> first, 
      IEnumerable<TSource> second
      ) where TSource: IEquatable<TSource>
    {
      using IEnumerator<TSource> e1 = first.GetEnumerator();
      using IEnumerator<TSource> e2 = second.GetEnumerator();
      
      while (e1.MoveNext())
      {
        if (!(e2.MoveNext() && e1.Current.Equals(e2.Current)))
        {
          return false;
        }
      }

      return !e2.MoveNext();
    }
    
    public static bool SequenceEqual<TSource>(
      in this ReadOnlySpan<TSource> first, 
      in ReadOnlySpan<TSource> second
    ) where TSource: IEquatable<TSource>
    {
      if (first.Length != second.Length)
      {
        return false;
      }
      
      for (ushort i =0; i<first.Length; ++i)
      {
        if (!first[i].Equals(second[i]))
        {
          return false;
        }
      }
      
      return true;
    }
  }
}