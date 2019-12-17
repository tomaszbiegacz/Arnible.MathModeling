using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class CollectionExtension
  {
    class BuildinComparerStruct<T> : IComparer<T> where T : struct, IComparable<T>
    {
      public int Compare(T x, T y) => x.CompareTo(y);
    }

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

    public static IEnumerable<T> Order<T>(this IEnumerable<T> collection) where T : struct, IComparable<T>
    {
      return collection.OrderBy(i => i, new BuildinComparerStruct<T>());
    }
  }
}
