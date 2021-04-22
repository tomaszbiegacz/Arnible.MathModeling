using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ZipExtensions
  {
    /// <summary>
    /// Applies a specified function to the corresponding elements of two sequences,
    /// producing a sequence of the results.
    /// </summary>
    public static IEnumerable<TResult> ZipValue<T, TResult>(
      this IEnumerable<T> col1,
      IEnumerable<T> col2,
      Func<T?, T?, TResult> merge) where T: struct
    {
      using (var col1Enum = col1.GetEnumerator())
      using (var col2Enum = col2.GetEnumerator())
      {
        bool isCol1Valid = col1Enum.MoveNext();
        bool isCol2Valid = col2Enum.MoveNext();
        while (isCol1Valid || isCol2Valid)
        {
          T? col1Current = null;
          if (isCol1Valid)
          {
            col1Current = col1Enum.Current;
            isCol1Valid = col1Enum.MoveNext();
          }

          T? col2Current = null;
          if (isCol2Valid)
          {
            col2Current = col2Enum.Current;
            isCol2Valid = col2Enum.MoveNext();
          }

          yield return merge(col1Current, col2Current);
        }
      }
    }
  }
}