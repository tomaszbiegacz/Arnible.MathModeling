using Arnible.MathModeling.Algebra;
using System;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static Polynomial ToPolar(this Polynomial source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint)
    {
      var x = (PolynomialTerm)cartesianPoint.X;
      var y = (PolynomialTerm)cartesianPoint.Y;

      var r = (PolynomialTerm)polarPoint.R;
      var φ = (PolynomialTerm)polarPoint.Φ;

      return source.Composition(x, r * Cos(φ)).Composition(y, r * Sin(φ));
    }

    public static HypersphericalCoordinate ToSpherical(this CartesianCoordinate p)
    {
      throw new NotImplementedException("Roots are not yet supported");
    }

    public static Number ToPolar(this Number source, RectangularCoordianate cartesianPoint, PolarCoordinate polarPoint)
    {
      return ToPolar((Polynomial)source, cartesianPoint, polarPoint);
    }

    public static PolynomialDivision ToSpherical(this PolynomialDivision source, CartesianCoordinate cartesianPoint, HypersphericalCoordinate hypersphericalPoint)
    {
      if (cartesianPoint.DimensionsCount != hypersphericalPoint.DimensionsCount)
      {
        throw new ArgumentException($"Invalid dimensions count");
      }
      
      PolynomialDivision replacement = (PolynomialDivision)hypersphericalPoint.R;
      PolynomialDivision result = source;

      NumberArray cd = cartesianPoint.Coordinates.Reverse().ToNumberArray();
      NumberArray ad = hypersphericalPoint.Angles.Reverse().ToNumberArray();
      for (uint i = 0; i < ad.Length; ++i)
      {
        var cartesianDimension = (PolynomialTerm)cd[i];
        var angle = (PolynomialTerm)ad[i];

        result = result.Composition(cartesianDimension, replacement * Sin(angle));
        replacement *= Cos(angle);
      }

      return result.Composition((PolynomialTerm)cd.Last(), replacement);
    }

    public static Number ToSpherical(this Number source, CartesianCoordinate cartesianPoint, HypersphericalCoordinate hypersphericalPoint)
    {
      return ToSpherical((PolynomialDivision)source, cartesianPoint, hypersphericalPoint);
    }

    public static HypersphericalCoordinateOnAxisView ToSphericalView(this CartesianCoordinate p)
    {
      throw new NotImplementedException("Not yet supported");
    }

    public static PolynomialDivision Composition(this Number source, CartesianCoordinate c1, CartesianCoordinate c2)
    {
      if(c1.DimensionsCount != c2.DimensionsCount)
      {
        throw new ArgumentException("Coordinates are with different dimensions");
      }

      PolynomialDivision result = (PolynomialDivision)source;
      for(uint i=0; i<c1.Coordinates.Length; ++i)
      {
        var cartesianDimension = (PolynomialTerm)c1.Coordinates[i];
        result = result.Composition(cartesianDimension, (PolynomialDivision)c2.Coordinates[i]);
      }
      return result;
    }
  }
}
