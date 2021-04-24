using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ExcludeAtExtensions
  {
    /// <summary>
    /// Return sequence without element at give position
    /// and throws an error if pos is greater than sequence length.
    /// </summary>
    public static IEnumerable<T> ExcludeAt<T>(this IEnumerable<T> x, uint pos)
    {
      bool isSkipped = false;
      uint i = 0;
      foreach (T item in x)
      {
        if (i == pos)
        {
          isSkipped = true;
        }
        else
        {
          yield return item;
        }
        i++;
      }
      if (!isSkipped)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't exclude at {pos}");
      }
    }
  }
}