using System;

namespace Arnible.MathModeling
{
  public class DerivativeValue : Derivative, IDerivative
  {
    public DerivativeValue(double first, double second)
    {
      if (double.IsNaN(first) || double.IsInfinity(first))
      {
        throw new ArgumentException(nameof(first));
      }
      if (double.IsNaN(second) || double.IsInfinity(second))
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
