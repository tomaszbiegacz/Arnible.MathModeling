using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public class DoubleEqualityComparer : IEqualityComparer<double>
  {
    public bool Equals(double x, double y) => NumericOperator.Equals(x, y);

    public int GetHashCode(double obj) => obj.GetHashCode();
  }
}
