using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LastExtensions
  {
    public static T Last<T>(this IEnumerable<T> source)
    {
      bool found = false;
#pragma warning disable CS8600
      T result = default;
#pragma warning restore CS8600      
      foreach (T val in source)
      {
        result = val;
        found = true;
      }
      
      if(found)
      {
#pragma warning disable 8603
        return result;
#pragma warning restore 8603
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
  }
}