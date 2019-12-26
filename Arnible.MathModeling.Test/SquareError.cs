using Arnible.MathModeling.Geometry;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Test
{
  public struct SquareError : IBinaryOperation<Number>
  {    
    public Number Value(Number x, Number y)
    {
      return (x - y).ToPower(2);
    }

    public IDerivative2 DerivativeByX(RectangularCoordianate p)
    {
      return new Derivative2Lazy(
        first: 2 * (p.X - p.Y),
        second: () => 2);
    }

    public IDerivative2 DerivativeByY(RectangularCoordianate p)
    {
      return new Derivative2Lazy(
        first: -2 * (p.X - p.Y),
        second: () => 2);
    }

    public IDerivative2 DerivativeByR(PolarCoordinate p)
    {
      return new Derivative2Lazy(
        first: 2 * p.R * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2),
        second: () => 2 * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2)
        );
    }     

    public IDerivative2 DerivativeByΦ(PolarCoordinate p)
    {
      return new Derivative2Lazy(
        first: 2 * p.R.ToPower(2) * (Sin(p.Φ).ToPower(2) - Cos(p.Φ).ToPower(2)),
        second: () => 8 * p.R.ToPower(2) * Cos(p.Φ) * Sin(p.Φ)
        );
    }
  }
}
