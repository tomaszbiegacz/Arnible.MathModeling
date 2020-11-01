using System;

namespace Arnible.MathModeling.Algebra
{
  public static class SignExtensions
  {
    private static Number ToValue(this Sign sign)
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

    public static ValueArray<Number> ToDirectionArray(this in UnmanagedArray<Sign> signs)
    {
      return signs.Select(s => ToValue(s)).ToValueArray();
    }
    
    public static ValueArray<Number> ToDirectionArray(this Sign[] signs)
    {
      return signs.Select(s => ToValue(s)).ToValueArray();
    }
  }
}