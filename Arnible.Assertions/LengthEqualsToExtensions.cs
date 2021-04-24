using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class LengthEqualsToExtensions
  {
    public static void AssertLengthEqualsTo<T>(this IReadOnlyCollection<T> src, IReadOnlyCollection<T> destination)
    {
      if(src.Count != destination.Count)
      {
        throw new AssertException(
          $"Expected length {src.Count} got {destination.Count}",
          AssertException.ToString(src.ToArray()));
      }
    }
    
    public static void AssertLengthEqualsTo<T>(in this ReadOnlySpan<T> src, ushort length)
    {
      if(src.Length != length)
      {
        throw new AssertException(
          $"Expected length {length} got {src.Length}",
          AssertException.ToString(src.ToArray()));
      }
    }
  }
}