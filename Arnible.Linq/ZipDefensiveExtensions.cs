using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ZipDefensiveExtensions
  {
    /// <summary>
    /// Applies a specified function to the corresponding elements of two sequences,
    /// producing a sequence of the results.
    /// </summary>
    /// <remarks>
    /// If validation of equal length is not needed, use Zip instead.
    /// </remarks>
    public static IEnumerable<TResult> ZipDefensive<T, TResult>(
      this IEnumerable<T> col1,
      IEnumerable<T> col2,
      Func<T, T, TResult> merge)
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
  }
}