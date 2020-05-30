using System;
using System.Collections.Generic;

//
// When changing anything here, change also LinqAggregateBasic
//

namespace Arnible.MathModeling
{
  public static class LinqAggregateBasicNumber
  {
    /// <summary>
    /// Finds minimum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static Number MinDefensive(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      Number? result = null;
      foreach (Number v in x)
      {
        if (result.HasValue)
        {
          if (v < result.Value)
          {
            result = v;
          }
        }
        else
        {
          result = v;
        }
      }
      if (!result.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return result.Value;
    }

    /// <summary>
    /// Finds record with minimum value
    /// </summary>
    public static T WithMinimum<T>(this IEnumerable<T> x, Func<T, Number> func) where T : struct
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      T? result = null;
      Number resultMinimum = default;
      foreach (T v in x)
      {
        if (result.HasValue)
        {
          Number value = func(v);
          if (value < resultMinimum)
          {
            result = v;
            resultMinimum = value;
          }
        }
        else
        {
          result = v;
          resultMinimum = func(v);
        }
      }
      if (!result.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return result.Value;
    }

    /// <summary>
    /// Finds position of record with minimum value
    /// </summary>
    public static uint WithMinimumAt<T>(this IEnumerable<T> x, Func<T, Number> func) where T : struct
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      uint? resultMinimumAt = null;
      Number resultMinimum = default;
      uint i = 0;
      foreach (T v in x)
      {
        if (resultMinimumAt.HasValue)
        {
          Number value = func(v);
          if (value < resultMinimum)
          {            
            resultMinimum = value;
            resultMinimumAt = i;
          }
        }
        else
        {
          resultMinimum = func(v);
          resultMinimumAt = i;
        }
        i++;
      }
      if (!resultMinimumAt.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return resultMinimumAt.Value;
    }

    /// <summary>
    /// Finds maximum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static Number MaxDefensive(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      Number? result = null;
      foreach (Number v in x)
      {
        if (result.HasValue)
        {
          if (v > result.Value)
          {
            result = v;
          }
        }
        else
        {
          result = v;
        }
      }
      if (!result.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return result.Value;
    }

    /// <summary>
    /// Finds record with minimum value
    /// </summary>
    public static T WithMaximum<T>(this IEnumerable<T> x, Func<T, Number> func) where T : struct
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      T? result = null;
      Number resultMaximum = default;
      foreach (T v in x)
      {
        if (result.HasValue)
        {
          Number value = func(v);
          if (value > resultMaximum)
          {
            result = v;
            resultMaximum = value;
          }
        }
        else
        {
          result = v;
          resultMaximum = func(v);
        }
      }
      if (!result.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return result.Value;
    }

    /// <summary>
    /// Finds position of record with maximum value
    /// </summary>
    public static uint WithMaximumAt<T>(this IEnumerable<T> x, Func<T, Number> func) where T : struct
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      uint? resultMaximumAt = null;
      Number resultMaximum = default;
      uint i = 0;
      foreach (T v in x)
      {
        if (resultMaximumAt.HasValue)
        {
          double value = func(v);
          if (value > resultMaximum)
          {
            resultMaximum = value;
            resultMaximumAt = i;
          }
        }
        else
        {
          resultMaximum = func(v);
          resultMaximumAt = i;
        }
        i++;
      }
      if (!resultMaximumAt.HasValue)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return resultMaximumAt.Value;
    }

    /// <summary>
    /// Finds median value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static Number MedianDefensive(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }

      Number[] items = x.ToArray();
      if (items.Length == 0)
      {
        throw new ArgumentNullException(nameof(x));
      }
      Array.Sort(items);
      return items[items.Length / 2];
    }
  }
}
