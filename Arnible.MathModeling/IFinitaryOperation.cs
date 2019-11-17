using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IFinitaryOperation
  {
    double Value(IEnumerable<double> x);
  }

  public interface IFinitaryOperationWithDerivative : IFinitaryOperation
  {
    IDerivative DerivativeBy(uint pos, params double[] x);
  }
}
