using System;

namespace Arnible.MathModeling
{
  public static class DoubleExtension
  {
    public static bool NumericEquals(this double a, double b)
    {
      if (IsValidNumeric(a) && IsValidNumeric(b))
      {
        if (a == b)
        {
          return true;
        }
        else
        {          
          double diff = Math.Abs(a - b);
          double denominator = Math.Min(Math.Abs(a), Math.Abs(b));
          const double resolution = 1E-10;
          if (denominator == 0)
          {
            return diff < resolution;
          }            
          else
          {
            double relativeDifference = diff / denominator;
            return relativeDifference < resolution;
          }
        }
      }
      else
      {
        return false;
      }
    }

    public static bool IsValidNumeric(this double a)
    {
      return !double.IsNaN(a) && !double.IsInfinity(a);
    }    

    public static double ToPower(this double a, uint b)
    {
      if (a == 1)
      {
        return 1;
      }

      switch (b)
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
