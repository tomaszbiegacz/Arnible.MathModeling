using Arnible.MathModeling.Analysis;
using Arnible.MathModeling.Geometry;
using static Arnible.MathModeling.NumberMath;

namespace Arnible.MathModeling.Test
{
  public class SquareError : IBinaryOperation<Number>
  {    
    public Number Value(in Number x, in Number y)
    {
      return (x - y).ToPower(2);
    }

    public Derivative2Value DerivativeByX(in RectangularCoordinate p)
    {
      return new Derivative2Value
      {
        First = 2 * (p.X - p.Y),
        Second = 2
      };
    }

    public Derivative2Value DerivativeByY(in RectangularCoordinate p)
    {
      return new Derivative2Value
      {
        First = -2 * (p.X - p.Y),
        Second = 2
      };
    }

    public Derivative2Value DerivativeByR(in PolarCoordinate p)
    {
      return new Derivative2Value
      {
        First = 2 * p.R * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2),
        Second = 2 * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2)
      };
    }     

    public Derivative2Value DerivativeByΦ(in PolarCoordinate p)
    {
      return new Derivative2Value
      {
        First = 2 * p.R.ToPower(2) * (Sin(p.Φ).ToPower(2) - Cos(p.Φ).ToPower(2)),
        Second = 8 * p.R.ToPower(2) * Cos(p.Φ) * Sin(p.Φ)
      };
    }
  }
}
