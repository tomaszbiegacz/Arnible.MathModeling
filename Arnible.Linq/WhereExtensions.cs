using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WhereExtensions
  {
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach(T item in source)
      {
        if(predicate(item))
        {
          yield return item;
        }
      }
    }
    
    public static bool Where<T>(
      in this ReadOnlySpan<T> source, 
      FuncIn<T, bool> predicate,
      in Span<T> output,
      in T defaultValue)
    {
      if(source.Length != output.Length)
      {
        throw new ArgumentException(nameof(output));
      }
      
      bool result = false;
      for(ushort i =0; i<source.Length; ++i)
      {
        if(predicate(in source[i]))
        {
          output[i] = source[i];
          result = true;
        }
        else
        {
          output[i] = defaultValue;
        }
      }
      return result;
    }
  }
}