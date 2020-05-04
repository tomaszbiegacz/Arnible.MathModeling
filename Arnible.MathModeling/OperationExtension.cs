using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class OperationExtension
  {
    public static double Value(this IFinitaryOperation<double> operation, params double[] x)
    {
      return operation.Value((IEnumerable<double>)x);
    }

    public static IFinitaryOperation<double> GetOperation(this IPolynomialOperation operation, params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(operation, variables.Select(pt => (char)pt));
    }
  }
}
