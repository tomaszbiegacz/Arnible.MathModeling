using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqComparable
  {
    /*
     * Filtering
     */
    
    private static bool IsPresent<T>(in List<T> result, in T item) where T : IEquatable<T>
    {
      foreach (T accepted in result)
      {
        if (accepted.Equals(item))
        {
          return true;
        }
      }
      return false;
    }
    
    public static IReadOnlyCollection<T> Distinct<T>(this IEnumerable<T> collection) where T: IEquatable<T>
    {
      List<T> result = new List<T>();
      foreach (T candidate in collection)
      {
        if (!IsPresent(result, in candidate))
        {
          result.Add(candidate);
        }
      }

      return result;
    }
    
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
