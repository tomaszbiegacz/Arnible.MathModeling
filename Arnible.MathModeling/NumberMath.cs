using System;
using Arnible.Assertions;

namespace Arnible.MathModeling
{
  public static class NumberMath
  {
    public static double RoundedSin(in double value)
    {
      if (value.NumericEquals(0) || value.NumericEquals(Math.PI)) return 0;
      else if (value.NumericEquals(Angle.RightAngle)) return 1;
      else if (value.NumericEquals(-1 * Angle.RightAngle)) return -1;
      else return Math.Sin(value);
    }

    public static double RoundedCos(in double value)
    {
      if (value.NumericEquals(0)) return 1;
      else if (value.NumericEquals(Math.PI)) return -1;
      else if (Math.Abs(value).NumericEquals(Angle.RightAngle)) return 0;
      else return Math.Cos(value);
    }
    
    public static Number Sqrt(in Number x)
    {
      if(x == 0)
        return 0;
      else
      {
        x.AssertIsGreaterThan(0);
        return Math.Sqrt((double)x);
      }
    }
    
    public static Number Exp(in Number x)
    {
      if(x == 0)
        return 1;
      else
      {
        return Math.Exp((double)x);
      }
    }
    
    public static Number Sin(in Number a) => RoundedSin((double)a);

    public static Number Cos(in Number a) => RoundedCos((double)a);
    
    public static Number Abs(this in Number a)
    {
      return Math.Abs((double)a);
    }
    
    public static Number ToPower(this in Number a, ushort b) => DoubleExtension.ToPower((double)a, b);
  }
}
