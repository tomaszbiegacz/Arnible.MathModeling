using System.Collections.Generic;
using System.Linq;
using Arnible.Assertions;

namespace Arnible.MathModeling
{
  public static class SequenceEqualExtensions
  {
    public static void AssertSequenceEqual(this IEnumerable<Number> actual, IReadOnlyList<double> expected)
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