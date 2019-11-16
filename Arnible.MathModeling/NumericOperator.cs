using System;

namespace Arnible.MathModeling
{
  public static class NumericOperator
  {
    public static bool IsValidNumeric(this double a)
    {
      return !Double.IsNaN(a) && !Double.IsInfinity(a);
    }

    public static bool Equals(double a, double b)
    {
      if (a == b)
      {
        return true;
      }
      else
      {
        double diff = Math.Abs(a - b);
        double denominator = Math.Min(a, b);
        return (denominator == 0 ? diff : diff / denominator) < 2E-16;
      }
    }
  }
}
