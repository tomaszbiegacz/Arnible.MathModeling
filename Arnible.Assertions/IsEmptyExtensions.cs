using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class IsEmptyExtensions
  {
    public static void AssertIsEmpty<T>(this IEnumerable<T> actual)
    {
      AssertIsEmpty(actual.ToArray());
    }
    
    public static void AssertIsEmpty(this string actual)
    {
      if(actual.Length != 0)
      {
        throw new AssertException(
          $"Expected empty got {actual}", 
          actual
        );
      }
    }
    
    public static void AssertIsEmpty<T>(this IReadOnlyCollection<T> actual)
    {
      if(actual.Count != 0)
      {
        throw new AssertException(
          $"Expected empty got {actual.Count} items", 
          AssertException.ToString(actual)
        );
      }
    }
  }
}