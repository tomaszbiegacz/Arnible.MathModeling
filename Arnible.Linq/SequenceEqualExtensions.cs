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
  }
}