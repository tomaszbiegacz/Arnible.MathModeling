using System;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class SignExtensions
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
  }
}