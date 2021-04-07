using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class EmptyExtensions
  {
    public static void AssertEmpty<T>(this IEnumerable<T> actual)
    {
      var actualMaterialized = actual.ToArray();
      if(actualMaterialized.Length != 0)
      {
        throw new AssertException(
          $"Expected empty got {actualMaterialized.Length} items", 
          AssertException.ToString(actualMaterialized)
        );
      }
    }
  }
}