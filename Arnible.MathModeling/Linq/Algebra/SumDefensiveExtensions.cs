using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.MathModeling;
using Arnible.MathModeling.Algebra;

namespace Arnible.Linq.Algebra
{
  public static class SumDefensiveExtensions
  {
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T SumDefensive<T>(this IEnumerable<T> x) where T: struct, IAlgebraGroup<T>
    {
      T? current = null;
      foreach (T v in x)
      {
        if(current.HasValue)
        {
          current =  current.Value.Add(in v);  
        }
        else
        {
          current = v;
        }
      }
      
      if (current.HasValue)
      {
        return current.Value;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T SumDefensive<T>(in this ReadOnlySpan<T> x) where T: struct, IAlgebraGroup<T>
    {
      T? current = null;
      foreach (ref readonly T v in x)
      {
        if(current.HasValue)
        {
          current =  current.Value.Add(in v);  
        }
        else
        {
          current = v;
        }
      }
      
      if (current.HasValue)
      {
        return current.Value;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static Number SumDefensive<T>(in this ReadOnlySpan<T> x, FuncIn<T, Number> getItem)
    {
      Number? current = null;
      foreach (ref readonly T v in x)
      {
        if(current.HasValue)
        {
          current =  current + getItem(in v);  
        }
        else
        {
          current = getItem(in v);
        }
      }
      
      if (current.HasValue)
      {
        return current.Value;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
  }
}