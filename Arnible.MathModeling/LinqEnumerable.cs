using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqEnumerable
  {
    /*
     * Extraction
     */

    public static T First<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.First(source);
    }

    public static T FirstOrDefault<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.FirstOrDefault(source);
    }
    
    public static T Last<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Last(source);
    }

    /// <summary>
    /// Returns the only element of a sequence
    /// and throws an exception if there is not exactly one item in sequence.
    /// </summary>
    public static T Single<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Single(source);
    }

    /// <summary>
    /// Returns the only element of a sequence or default 
    /// and throws an exception if there are more than one element in sequence.
    /// </summary>
    public static T SingleOrDefault<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.SingleOrDefault(source);
    }

    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, in Func<T, bool> predicate)
    {
      return System.Linq.Enumerable.Where(source, predicate);
    }


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

    public static UnmanagedArray<T> ToUnmanagedArray<T>(this IEnumerable<T> source) where T : unmanaged
    {
      return new UnmanagedArray<T>(System.Linq.Enumerable.ToArray(source));
    }

    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToArray(source);
    }

    public static List<T> ToList<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToList(source);
    }

    public static IDictionary<TKey, TSource> ToDictionary<TKey, TSource>(
      this IEnumerable<TSource> source, 
      in Func<TSource, TKey> keySelector)
    {
      return System.Linq.Enumerable.ToDictionary(source, keySelector);
    }

    public static IDictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
      this IEnumerable<TSource> source,
      in Func<TSource, TKey> keySelector,
      in Func<TSource, TValue> valueSelector)
    {
      return System.Linq.Enumerable.ToDictionary(source, keySelector, valueSelector);
    }

    /*
     * Creation
     */

    public static IEnumerable<T> Yield<T>(T src)
    {
      yield return src;
    }

    public static IEnumerable<int> RangeInt(uint length) => RangeInt(0, length);

    public static IEnumerable<int> RangeInt(int start, uint length)
    {
      for (int i = start; i < start + length; ++i)
      {
        yield return i;
      }
    }

    public static IEnumerable<uint> RangeUint(uint length) => RangeUint(0, length);

    public static IEnumerable<uint> RangeUint(uint start, uint length)
    {
      for (uint i = start; i < start + length; ++i)
      {
        yield return i;
      }
    }

    public static IEnumerable<T> Repeat<T>(T item, uint length) where T : struct
    {
      for (uint i = 0; i < length; ++i)
      {
        yield return item;
      }
    }

    public static IEnumerable<T> Empty<T>() => System.Linq.Enumerable.Empty<T>();

    /*
     * Adding items
     */

    public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item)
    {
      return System.Linq.Enumerable.Append(src, item);
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, IEnumerable<T> after)
    {
      return System.Linq.Enumerable.Concat(source, after);
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
      uint i = 0;
      foreach (T item in x)
      {
        if (i < count)
        {
          yield return item;
        }
        i++;
      }
      if (i < count)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't take exactly {count}");
      }
    }

    public static IEnumerable<T> TakeAtMost<T>(this IEnumerable<T> x, uint count)
    {
      uint i = 0;
      foreach (T item in x)
      {
        if (i < count)
        {
          yield return item;
        }
        i++;
      }
    }
  }
}
