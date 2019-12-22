using System;

namespace Arnible.MathModeling
{
  public class Derivative2Lazy : IDerivative2
  {
    private Lazy<Number> _second;

    public Derivative2Lazy(Number first, Func<Number> second)
    {
      First = first;
      _second = new Lazy<Number>(second);
    }

    public Number First { get; }

    public Number Second => _second.Value;
  }
}
