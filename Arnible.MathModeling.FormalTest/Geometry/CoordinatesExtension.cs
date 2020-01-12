using Arnible.MathModeling.Algebra;
using System;
using System.Linq;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    public static Polynomial ToPolar(this Polynomial source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint)
    {
      var x = (PolynomialTerm)cartesianPoint.X;
      var y = (PolynomialTerm)cartesianPoint.Y;

      var r = (PolynomialTerm)polarPoint.R;
      var φ = (PolynomialTerm)polarPoint.Φ;

      return source.Composition(x, r * Cos(φ)).Composition(y, r * Sin(φ));
    }

    public static Number ToPolar(this Number source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint) => ToPolar((Polynomial)source, cartesianPoint, polarPoint);

    public static Polynomial ToSpherical(this Polynomial source, CartesianCoordinate cartesianPoint, HypersphericalCoordinate hypersphericalPoint)
    {
      if (cartesianPoint.DimensionsCount != hypersphericalPoint.DimensionsCount)
      {
        throw new ArgumentException($"Invalid dimensions count");
      }

      Polynomial replacement = hypersphericalPoint.R;
      var result = source;

      NumberVector cd = cartesianPoint.Coordinates.Reverse();
      NumberVector ad = hypersphericalPoint.Angles.Reverse();
      for (uint i = 0; i < ad.Count; ++i)
      {
        var cartesianDimension = (PolynomialTerm)cd[i];
        var angle = (PolynomialTerm)ad[i];

        result = result.Composition(cartesianDimension, replacement * Cos(angle));
        replacement *= Sin(angle);
      }

      return result.Composition((PolynomialTerm)cd.Last(), replacement);
    }

    public static Number ToSpherical(this Number source, CartesianCoordinate cartesianPoint, HypersphericalCoordinate hypersphericalPoint) => ToSpherical((Polynomial)source, cartesianPoint, hypersphericalPoint);
  }
}
