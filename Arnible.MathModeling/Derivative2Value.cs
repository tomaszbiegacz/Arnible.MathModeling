using System;

namespace Arnible.MathModeling
{
  public class Derivative2Value : Derivative1Value, IDerivative2
  {
    public Derivative2Value(Number first, Number second)
      : base(first)
    {
      if (!second.IsValidNumeric)
      {
        throw new ArgumentException(nameof(second));
      }

      Second = second;
    }

    public Number Second { get; }
  }
}
