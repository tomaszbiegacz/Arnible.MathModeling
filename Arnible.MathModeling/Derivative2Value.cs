using System;

namespace Arnible.MathModeling
{
  public class Derivative2Value : IDerivative2
  {
    public Derivative2Value(Number first, Number second)
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

    public Number First { get; }

    public Number Second { get; }
  }
}
