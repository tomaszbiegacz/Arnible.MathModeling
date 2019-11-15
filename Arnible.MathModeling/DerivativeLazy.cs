using System;

namespace Arnible.MathModeling
{
  public class DerivativeLazy : Derivative, IDerivative
  {
    private Lazy<double> _first;
    private Lazy<double> _second;

    public DerivativeLazy(Func<double> first, Func<double> second)
    {
      _first = new Lazy<double>(first);
      _second = new Lazy<double>(second);
    }

    public override double First => _first.Value;

    public override double Second => _second.Value;
  }
}
