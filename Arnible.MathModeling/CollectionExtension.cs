using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class CollectionExtension
  {
    public static IEnumerable<int> Indexes<T>(this T[] arg) => Enumerable.Range(0, arg.Length);    

    public static IEnumerable<T> ExcludeAt<T>(this IEnumerable<T> x, uint pos)
    {      
      bool isSkipped = false;
      var xEnumerator = x.GetEnumerator();
      uint i;
      for (i = 0; xEnumerator.MoveNext(); ++i)
      {
        if (i == pos)
        {
          isSkipped = true;
        }
        else
        {          
          yield return xEnumerator.Current;
        }
      }
      if(!isSkipped)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't exclude at {pos}");
      }
    }

    public static IEnumerable<T> ExcludeAt<T>(this T[] x, uint pos) => ExcludeAt((IEnumerable<T>)x, pos);
  }
}
