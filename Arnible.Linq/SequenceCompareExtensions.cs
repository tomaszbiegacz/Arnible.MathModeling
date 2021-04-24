using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SequenceCompareExtensions
  {
    public static int SequenceCompare<T>(this IEnumerable<T> col1, IEnumerable<T> col2) where T : struct, IComparable<T>
    {
      using var col1Enum = col1.GetEnumerator();
      using var col2Enum = col2.GetEnumerator();
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

      // there is no difference
      return 0;
    }
  }
}