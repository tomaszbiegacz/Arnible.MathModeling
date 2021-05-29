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