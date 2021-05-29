using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class FirstExtensions
  {
    public static T First<T>(this IEnumerable<T> source)
    {
      foreach(T val in source)
      {
        return val;
      }
      throw new ArgumentException(nameof(source));
    }
  }
}