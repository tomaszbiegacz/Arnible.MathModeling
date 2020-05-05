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
  }
}
