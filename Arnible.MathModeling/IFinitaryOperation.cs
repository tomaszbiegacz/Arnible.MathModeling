using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IFinitaryOperation
  {
    double Value(params double[] x);

    double Value(IEnumerable<double> x);

    IDerivative DerivativeBy(uint pos, params double[] x);
  }
}
