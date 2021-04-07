using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class TakeExactlyExtensions
  {
    public static IEnumerable<T> TakeExactly<T>(this IEnumerable<T> x, uint count)
    {
      using var iterator = x.GetEnumerator();
      uint i = 0;
      bool isTheEnd = false;
      while (!isTheEnd)
      {
        if(iterator.MoveNext())
        {
          yield return iterator.Current;
          i++;
        }
        else
        {
          throw new ArgumentException($"Enumerator length {i}, hence I can't take exactly {count}");
        }
        isTheEnd = i >= count;
      }
    }
  }
}