using System;

namespace Arnible.MathModeling.Algebra
{
  public static class SignExtensions
  {
    public static Number ToValue(this Sign sign)
    {
      switch (sign)
      {
        case Sign.Negative:
          return -1;
        case Sign.None:
          return 0;
        case Sign.Positive:
          return 1;
      }

      throw new ArgumentException(nameof(sign));
    }
  }
}