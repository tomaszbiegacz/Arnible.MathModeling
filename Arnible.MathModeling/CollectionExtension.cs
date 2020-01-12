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

    public static IEnumerable<T> Yield<T>(this T src)
    {
      yield return src;
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

    public static IEnumerable<T> SelectMerged<T>(this IEnumerable<T> col1, IEnumerable<T> col2, Func<T, T, T> merge)
    {
      var col1Enum = col1.GetEnumerator();
      var col2Enum = col2.GetEnumerator();

      bool isCol1Valid = col1Enum.MoveNext();
      bool isCol2Valid = col2Enum.MoveNext();
      while (isCol1Valid && isCol2Valid)
      {
        yield return merge(col1Enum.Current, col2Enum.Current);

        isCol1Valid = col1Enum.MoveNext();
        isCol2Valid = col2Enum.MoveNext();
      }

      if(isCol1Valid || isCol2Valid)
      {
        throw new InvalidOperationException("Collections are not the same size.");
      }
    }
  }
}
