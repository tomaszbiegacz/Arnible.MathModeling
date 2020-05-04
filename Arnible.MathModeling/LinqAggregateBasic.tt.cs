using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqAggregateBasic
  {
    public static double MinDefensive(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      double? result = null;
      foreach (double v in x)
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

    public static double MaxDefensive(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      double? result = null;
      foreach (double v in x)
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
