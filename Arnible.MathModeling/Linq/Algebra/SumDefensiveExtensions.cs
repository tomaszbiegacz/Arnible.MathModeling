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
    public static T SumDefensive<T>(this IEnumerable<T> x) where T: IAlgebraGroup<T>
    {
      bool isCurrentSet = false;
#pragma warning disable 8600
      T current = default;
#pragma warning restore 8600
      foreach (T v in x)
      {
        if(isCurrentSet)
        {
#pragma warning disable 8602
          current =  current.Add(in v);  
#pragma warning restore 8602
        }
        else
        {
          current = v;
          isCurrentSet = true;
        }
      }
      
      if (isCurrentSet)
      {
#pragma warning disable 8603
        return current;
#pragma warning restore 8603
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T SumDefensive<T>(in this ReadOnlySpan<T> x) where T: IAlgebraGroup<T>
    {
      bool isCurrentSet = false;
#pragma warning disable 8600
      T current = default;
#pragma warning restore 8600
      foreach (ref readonly T v in x)
      {
        if(isCurrentSet)
        {
#pragma warning disable 8602
          current =  current.Add(in v);  
#pragma warning restore 8602
        }
        else
        {
          current = v;
          isCurrentSet = true;
        }
      }
      
      if (isCurrentSet)
      {
#pragma warning disable 8603
        return current;
#pragma warning restore 8603
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
    
    public static T SumDefensive<T>(in this Span<T> x) where T: IAlgebraGroup<T>
    {
      return SumDefensive((ReadOnlySpan<T>)x);
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
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static Number SumDefensive<T>(in this Span<T> x, FuncIn<T, Number> getItem)
    {
      return SumDefensive((ReadOnlySpan<T>)x, getItem);
    }
  }
}