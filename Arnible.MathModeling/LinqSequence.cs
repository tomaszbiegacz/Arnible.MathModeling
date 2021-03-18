using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling
{
  public static class LinqSequence
  {
    public static bool SequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> other)
    {
      return System.Linq.Enumerable.SequenceEqual(source, other);
    }

    public static IEnumerable<IEnumerable<T>> ToSequencesWithReturning<T>(this IEnumerable<T> items, in uint length)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      return ToSequencesWithReturningInternal(items.ToReadOnlyList(), length);
    }

    public static IEnumerable<IEnumerable<T>> ToSequencesWithReturning<T>(this IEnumerable<T> items)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToReadOnlyList();
      return ToSequencesWithReturningInternal(x, (uint)x.Count);
    }

    private static IEnumerable<IEnumerable<T>> ToSequencesWithReturningInternal<T>(IReadOnlyList<T> items, uint length)
    {
      if (length > 0)
      {
        if (length == 1)
        {
          foreach (T item in items)
          {
            yield return LinqEnumerable.Yield(item);
          }
        }
        else
        {
          for (int i = 0; i < items.Count; ++i)
          {
            T e = items[i];
            foreach (IEnumerable<T> combination in ToSequencesWithReturningInternal(items, length - 1))
            {
              yield return combination.Prepend(e);
            }
          }
        }
      }
    }    
  }
}
