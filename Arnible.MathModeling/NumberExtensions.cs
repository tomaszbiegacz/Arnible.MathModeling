using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumberExtensions
  {
    /// <summary>
    /// True if all values are zero or enumerator is empty
    /// </summary>
    public static bool IsZero(this IEnumerable<Number> values)
    {
      foreach (Number val in values)
      {
        if(val != 0)
        {
          return false;
        }
      }
      return true;
    }
  }
}