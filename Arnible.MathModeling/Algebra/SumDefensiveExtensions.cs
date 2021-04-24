using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
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
    public static T[] SumDefensive<T>(this IEnumerable<IReadOnlyCollection<T>> x) where T: struct, IAlgebraGroup<T>
    {
      T[]? current = null;
      foreach (IReadOnlyCollection<T> v in x)
      {
        if(current is not null)
        {
          current =  current.Add(v);  
        }
        else
        {
          current = v.ToArray();
        }
      }
      
      return current ?? throw new ArgumentException("Empty enumerator");
    }
  }
}