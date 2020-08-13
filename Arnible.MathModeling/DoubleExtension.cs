using System;

namespace Arnible.MathModeling
{
  public static class DoubleExtension
  {
    public static bool NumericEquals(in this double a, in double b)
    {
      if (IsValidNumeric(in a) && IsValidNumeric(in b))
      {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (a == b)
        {
          return true;
        }
        else
        {          
          double diff = Math.Abs(a - b);
          double denominator = Math.Min(Math.Abs(a), Math.Abs(b));
          const double resolution = 1E-9;
          // ReSharper disable once CompareOfFloatsByEqualityOperator
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

    public static bool IsValidNumeric(in this double a)
    {
      return !double.IsNaN(a) && !double.IsInfinity(a);
    }    

    public static double ToPower(in this double a, in uint b)
    {
      if (a.NumericEquals(1))
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

    public static double RoundedSin(in double value)
    {
      if (NumericEquals(in value, 0)) return 0;
      else if (NumericEquals(in value, Angle.RightAngle)) return 1;
      else return Math.Sin(value);
    }

    public static double RoundedCos(in double value)
    {
      if (NumericEquals(in value, 0)) return 1;
      else if (NumericEquals(in value, Angle.RightAngle)) return 0;
      else return Math.Cos(value);
    }
  }
}
