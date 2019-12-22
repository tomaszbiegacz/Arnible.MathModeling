using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class OperationExtension
  {
    public static double Value(this IFinitaryOperation<double> operation, params double[] x) => operation.Value((IEnumerable<double>)x);    

    public static IFinitaryOperation<double> GetOperation(this IPolynomialOperation operation, params PolynomialTerm[] variables) => new PolynomialFinitaryOperation(operation, variables.Select(pt => (char)pt));
  }
}
