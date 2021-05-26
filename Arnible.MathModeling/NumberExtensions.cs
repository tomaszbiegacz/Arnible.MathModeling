using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumberExtensions
  {
    public static Sign GetSign(this in Number v)
    {
      if (v == 0)
      {
        return Sign.None;
      }
      else
      {
        return v > 0 ? Sign.Positive : Sign.Negative;
      }
    }
    
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
    
    public static Number[] ToNumberArray(this IReadOnlyList<double> values)
    {
      Number[] result = new Number[values.Count];
      for(ushort i=0; i<values.Count; ++i)
      {
        result[i] = values[i];
      }
      return result;
    }
  }
}