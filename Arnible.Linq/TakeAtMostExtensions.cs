using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class TakeAtMostExtensions
  {
    public static IEnumerable<T> TakeAtMost<T>(this IEnumerable<T> x, uint count)
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
          isTheEnd = i >= count;
        }
        else
        {
          isTheEnd = true;
        }
      }
    }
  }
}