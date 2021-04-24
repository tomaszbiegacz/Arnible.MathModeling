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

    public Derivative2Value DerivativeByX(RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByY(RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: -2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByR(PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2),
        second: 2 * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2)
        );
    }     

    public Derivative2Value DerivativeByΦ(PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R.ToPower(2) * (Sin(p.Φ).ToPower(2) - Cos(p.Φ).ToPower(2)),
        second: 8 * p.R.ToPower(2) * Cos(p.Φ) * Sin(p.Φ)
        );
    }
  }
}
