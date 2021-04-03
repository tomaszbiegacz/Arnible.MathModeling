using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assert
{
  public static class SequenceEqualExtensions
  {
    public static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IReadOnlyList<T> expected)
      where T: IEquatable<T>
    {
      var actualMaterialized = actual.ToArray();
      if(actualMaterialized.Length != expected.Count)
      {
        throw new AssertException(
          $"Expected length {expected.Count} got {actualMaterialized.Length}", 
          AssertException.ToString(actualMaterialized)
          );
      }
      for(ushort i=0; i<expected.Count; ++i)
      {
        if(!actualMaterialized[i].Equals(expected[i]))
        {
          throw new AssertException(
            $"At position {i} expected {expected[i]} got {actualMaterialized[i]}", 
            AssertException.ToString(actualMaterialized)
            );
        }
      }
    }
  }
}