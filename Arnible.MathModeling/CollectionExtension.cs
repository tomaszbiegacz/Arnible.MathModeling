using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class CollectionExtension
  {
    /*
     * Single item extensions
     */

    public static IEnumerable<T> Yield<T>(this T src)
    {
      yield return src;
    }

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> src, T item)
    {
      yield return item;
      foreach (var srcItem in src)
      {
        yield return srcItem;
      }
    }

    /*
     * Array extensions
     */

    public static IEnumerable<int> Indexes<T>(this T[] arg) => Enumerable.Range(0, arg.Length);

    /*
     * IReadOnlyList extension
     */

    public static int IndexOf<T>(this IReadOnlyList<T> src, Func<T, bool> predicate)
    {
      for (int i = 0; i < src.Count; ++i)
      {
        if (predicate(src[i]))
        {
          return i;
        }
      }
      return -1;
    }

    /*
     * Enumerable
     */

    public static bool All(this IEnumerable<bool> col1)
    {
      using (var col1Enum = col1.GetEnumerator())
      {
        while (col1Enum.MoveNext())
        {
          if (!col1Enum.Current)
            return false;
        }
      }
      return true;
    }

    public static IEnumerable<T> ExcludeAt<T>(this IEnumerable<T> x, uint pos)
    {
      bool isSkipped = false;
      using (var xEnumerator = x.GetEnumerator())
      {
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
        if (!isSkipped)
        {
          throw new ArgumentException($"Enumerator length {i}, hence I can't exclude at {pos}");
        }
      }
    }

    class BuildinComparerStruct<T> : IComparer<T> where T : struct, IComparable<T>
    {
      public int Compare(T x, T y) => x.CompareTo(y);
    }

    public static IEnumerable<T> Order<T>(this IEnumerable<T> collection) where T : struct, IComparable<T>
    {
      return collection.OrderBy(i => i, new BuildinComparerStruct<T>());
    }

    public static int SequenceCompare<T>(this IEnumerable<T> col1, IEnumerable<T> col2) where T : struct, IComparable<T>
    {
      using (var col1Enum = col1.GetEnumerator())
      using (var col2Enum = col2.GetEnumerator())
      {
        bool isCol1Valid = col1Enum.MoveNext();
        bool isCol2Valid = col2Enum.MoveNext();
        while (isCol1Valid && isCol2Valid)
        {
          int result = col1Enum.Current.CompareTo(col2Enum.Current);
          if (result != 0)
          {
            // first difference
            return result;
          }

          isCol1Valid = col1Enum.MoveNext();
          isCol2Valid = col2Enum.MoveNext();
        }

        if (isCol1Valid || isCol2Valid)
        {
          throw new InvalidOperationException("Collections are not the same size.");
        }
      }

      // there is no difference
      return 0;
    }

    /// <summary>
    /// If validation of equal length is not needed, use Enumerable.Zip instead.
    /// </summary>    
    public static IEnumerable<TResult> ZipDefensive<T, TResult>(this IEnumerable<T> col1, IEnumerable<T> col2, Func<T, T, TResult> merge)
    {
      using (var col1Enum = col1.GetEnumerator())
      using (var col2Enum = col2.GetEnumerator())
      {
        bool isCol1Valid = col1Enum.MoveNext();
        bool isCol2Valid = col2Enum.MoveNext();
        while (isCol1Valid && isCol2Valid)
        {
          yield return merge(col1Enum.Current, col2Enum.Current);

          isCol1Valid = col1Enum.MoveNext();
          isCol2Valid = col2Enum.MoveNext();
        }

        if (isCol1Valid || isCol2Valid)
        {
          throw new InvalidOperationException("Collections are not the same size.");
        }
      }
    }

    public static IEnumerable<IEnumerable<T>> ToSequncesWithReturning<T>(this IEnumerable<T> items, uint length)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      return ToSequncesWithReturningInternal(items.ToList(), length);
    }

    public static IEnumerable<IEnumerable<T>> ToSequncesWithReturning<T>(this IEnumerable<T> items)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToList();
      return ToSequncesWithReturningInternal(x, (uint)x.Count);
    }

    private static IEnumerable<IEnumerable<T>> ToSequncesWithReturningInternal<T>(List<T> items, uint length)
    {
      if (length > 0)
      {
        if (length == 1)
        {
          foreach (var item in items)
          {
            yield return item.Yield();
          }
        }
        else
        {
          for (int i = 0; i < items.Count; ++i)
          {
            var e = items[i];
            foreach (IEnumerable<T> combination in ToSequncesWithReturningInternal(items, length - 1))
            {
              yield return combination.Prepend(e);
            }
          }
        }
      }
    }
  }
}
