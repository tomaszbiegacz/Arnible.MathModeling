using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public static class PolynomialExtension
  {
    public static Polynomial ToPolar(this Polynomial source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint)
    {
      var x = (PolynomialTerm)cartesianPoint.X;
      var y = (PolynomialTerm)cartesianPoint.Y;

      var φ = (PolynomialTerm)polarPoint.Φ;
      var r = (PolynomialTerm)polarPoint.R;

      return source.Composition(x, r * Cos(φ)).Composition(y, r * Sin(φ));
    }

    public static Number ToPolar(this Number source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint) => ToPolar((Polynomial)source, cartesianPoint, polarPoint);
  }
}
