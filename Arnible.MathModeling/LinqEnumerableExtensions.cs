using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class LinqEnumerableExtensions
  {
    /*
     * Materialization
     */

    public static ValueArray<T> ToValueArray<T>(this T[] source) where T : struct, IValueObject
    {
      return new ValueArray<T>(source);
    }

    public static ValueArray<T> ToValueArray<T>(this IEnumerable<T> source) where T : struct, IValueObject
    {
      return new ValueArray<T>(System.Linq.Enumerable.ToArray(source));
    }

    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToArray(source);
    }

    
    public static List<T> ToList<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToList(source);
    }



    /*
     * Adding items
     */

    public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item, uint count)
    {
      foreach (T srcItem in src)
      {
        yield return srcItem;
      }
      for (uint i = 0; i < count; i++)
      {
        yield return item;
      }
    }
    
    public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item)
    {
      return Append(src, item, 1);
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, IEnumerable<T> after)
    {
      foreach (T srcItem in source)
      {
        yield return srcItem;
      }
      foreach (T srcItem in after)
      {
        yield return srcItem;
      }
    }

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> src, T item)
    {
      yield return item;
      foreach (var srcItem in src)
      {
        yield return srcItem;
      }
    }

    public static IEnumerable<T> DuplicateAt<T>(this IEnumerable<T> x, uint pos)
    {
      bool isDuplicated = false;
      uint i = 0;
      foreach (T item in x)
      {
        yield return item;
        if (i == pos)
        {
          isDuplicated = true;
          yield return item;
        }
        i++;
      }
      if (!isDuplicated)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't duplicate at {pos}");
      }
    }

    public static IEnumerable<T> DuplicateAt<T>(this IEnumerable<T> x, IReadOnlyCollection<uint> pos)
    {
      uint i = 0;
      foreach (T item in x)
      {
        yield return item;
        if (System.Linq.Enumerable.Contains(pos, i))
        {
          yield return item;
        }
        i++;
      }
    }

    /*
     * Filtering out items
     */

    /// <summary>
    /// Return sequence without element at give position
    /// and throws an error if pos is greater than sequence length.
    /// </summary>
    public static IEnumerable<T> ExcludeAt<T>(this IEnumerable<T> x, uint pos)
    {
      bool isSkipped = false;
      uint i = 0;
      foreach (T item in x)
      {
        if (i == pos)
        {
          isSkipped = true;
        }
        else
        {
          yield return item;
        }
        i++;
      }
      if (!isSkipped)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't exclude at {pos}");
      }
    }

    /// <summary>
    /// Return sequence without element at give positions    
    /// </summary>
    public static IEnumerable<T> ExcludeAt<T>(this IEnumerable<T> x, IReadOnlyCollection<uint> pos)
    {
      uint i = 0;
      foreach (T item in x)
      {
        if (!System.Linq.Enumerable.Contains(pos, i))
        {
          yield return item;
        }
        i++;
      }
    }

    /// <summary>
    /// Return only the elements that can safely be cast to type x.
    /// </summary>    
    public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
    {
      return System.Linq.Enumerable.OfType<TResult>(source);
    }

    public static IEnumerable<T> SkipExactly<T>(this IEnumerable<T> x, uint length)
    {
      uint i = 0;
      foreach (T item in x)
      {
        if (i >= length)
        {
          yield return item;
        }
        i++;
      }
      if (i < length)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't skip {length}");
      }
    }

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
