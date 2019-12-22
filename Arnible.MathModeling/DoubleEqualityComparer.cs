using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public class DoubleEqualityComparer : IEqualityComparer<double>
  {
    public bool Equals(double x, double y) => x.NumericEquals(y);

    public int GetHashCode(double obj) => obj.GetHashCode();
  }
}
