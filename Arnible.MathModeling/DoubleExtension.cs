using System;

namespace Arnible.MathModeling
{
  public static class DoubleExtension
  {
    public static bool NumericEquals(in this double a, in double b)
    {
      if (IsValidNumeric(in a) && IsValidNumeric(in b))
      {
        if (a == b)
        {
          return true;
        }
        else
        {          
          double diff = Math.Abs(a - b);
          double denominator = Math.Min(Math.Abs(a), Math.Abs(b));
          const double resolution = 1E-9;
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

    public static double ToPower(in this double a, ushort b)
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
        default:
        {
          double temp = ToPower(a, (ushort)(b / 2));
          if (b % 2 == 0)
          {
            return temp*temp;
          }
          else
          {
           return temp*temp*a; 
          }
        }
      }
    }
  }
}
