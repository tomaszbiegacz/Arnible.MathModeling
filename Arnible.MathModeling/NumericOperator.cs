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

    public static double Power(double a, uint b)
    {
      switch(b)
      {
        case 0:
          return 1;
        case 1:
          return a;
        case 2:
          return a * a;
        case 3:
          return a * a * a;
        case 4:
          return a * a * a * a;
        case 5:
          return a * a * a * a * a;
        default:
          return Math.Pow(a, b);
      }
    }
  }
}
