using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class OperationExtension
  {
    public static double Value(this IFinitaryOperation operation, params double[] x) => operation.Value((IEnumerable<double>)x);

    public static IFinitaryOperation GetOperation(this IPolynomialOperation operation, params char[] variables) => new PolynomialFinitaryOperation(operation, variables);
  }
}
