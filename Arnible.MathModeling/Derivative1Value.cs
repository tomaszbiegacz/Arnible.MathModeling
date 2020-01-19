using System;

namespace Arnible.MathModeling
{
  public class Derivative1Value : IDerivative1
  {
    public Derivative1Value(Number first)
    {
      First = first;
    }

    public Number First { get; }
  }
}
