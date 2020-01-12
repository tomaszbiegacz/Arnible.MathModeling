using System;

namespace Arnible.MathModeling
{
  public class Derivative1Value : IDerivative1
  {
    public Derivative1Value(Number first)
    {
      if (!first.IsValidNumeric)
      {
        throw new ArgumentException(nameof(first));
      }

      First = first;
    }

    public Number First { get; }
  }
}
