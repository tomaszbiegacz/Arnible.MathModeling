using System;
using System.Collections.Generic;

namespace Arnible.Linq.Combinatorics
{
  public static class ToSequencesExtensions
  {
    public static IEnumerable<IEnumerable<T>> ToSequencesWithReturning<T>(this IEnumerable<T> items, in uint length)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      return ToSequencesWithReturningInternal(items.ToArray(), length);
    }

    public static IEnumerable<IEnumerable<T>> ToSequencesWithReturning<T>(this IEnumerable<T> items)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToArray();
      return ToSequencesWithReturningInternal(x, (uint)x.Length);
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