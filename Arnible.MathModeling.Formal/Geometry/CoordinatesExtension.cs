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
      NumberVector ad = hypersphericalPoint.Angles.Reverse().ToVector();
      for (uint i = 0; i < ad.Count; ++i)
      {
        var cartesianDimension = (PolynomialTerm)cd[i];
        var angle = (PolynomialTerm)ad[i];

        result = result.Composition(cartesianDimension, replacement * Sin(angle));
        replacement *= Cos(angle);
      }

      return result.Composition((PolynomialTerm)cd.Last(), replacement);
    }

    public static Number ToSpherical(this Number source, CartesianCoordinate cartesianPoint, HypersphericalCoordinate hypersphericalPoint) => ToSpherical((Polynomial)source, cartesianPoint, hypersphericalPoint);

    public static Polynomial Composition(this Polynomial source, CartesianCoordinate c1, CartesianCoordinate c2)
    {
      if(c1.DimensionsCount != c2.DimensionsCount)
      {
        throw new ArgumentException("Coordinates are with different dimensions");
      }

      var result = source;
      for(uint i=0; i<c1.Coordinates.Count; ++i)
      {
        var cartesianDimension = (PolynomialTerm)c1.Coordinates[i];
        result = result.Composition(cartesianDimension, c2.Coordinates[i]);
      }
      return result;
    }
  }
}
