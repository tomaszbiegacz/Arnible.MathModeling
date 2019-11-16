using System;

namespace Arnible.MathModeling
{
  public class DerivativeValue : Derivative, IDerivative
  {
    public DerivativeValue(double first, double second)
    {
      if (!first.IsValidNumeric())
      {
        throw new ArgumentException(nameof(first));
      }
      if (!second.IsValidNumeric())
      {
        throw new ArgumentException(nameof(second));
      }

      First = first;
      Second = second;
    }

    public override double First { get; }

    public override double Second { get; }
  }
}
