using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Polynomials;
using System;
using static Arnible.MathModeling.Polynomials.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static Polynomial ToPolar(in this Polynomial source, in RectangularCoordianate cartesianPoint, in PolarCoordinate polarPoint)
    {
      var x = (PolynomialTerm)cartesianPoint.X;
      var y = (PolynomialTerm)cartesianPoint.Y;

      var r = (PolynomialTerm)polarPoint.R;
      var φ = (PolynomialTerm)polarPoint.Φ;

      return source.Composition(x, r * Cos(φ)).Composition(y, r * Sin(φ));
    }

    public static HypersphericalCoordinate ToSpherical(in this CartesianCoordinate p)
    {
      throw new NotImplementedException("Roots are not yet supported");
    }

    public static Number ToPolar(in this Number source, in RectangularCoordianate cartesianPoint, in PolarCoordinate polarPoint)
    {
      return ToPolar((Polynomial)source, in cartesianPoint, in polarPoint);
    }

    public static PolynomialDivision ToSpherical(
      in this PolynomialDivision source, 
      in CartesianCoordinate cartesianPoint, 
      in HypersphericalCoordinate hypersphericalPoint)
    {
      if (cartesianPoint.DimensionsCount != hypersphericalPoint.DimensionsCount)
      {
        throw new ArgumentException($"Invalid dimensions count");
      }
      
      PolynomialDivision replacement = (PolynomialDivision)hypersphericalPoint.R;
      PolynomialDivision result = source;

      ValueArray<Number> cd = cartesianPoint.Coordinates.Reverse().ToValueArray();
      ValueArray<Number> ad = hypersphericalPoint.Angles.Reverse().ToValueArray();
      for (uint i = 0; i < ad.Length; ++i)
      {
        var cartesianDimension = (PolynomialTerm)cd[i];
        var angle = (PolynomialTerm)ad[i];

        result = result.Composition(cartesianDimension, replacement * Sin(angle));
        replacement *= Cos(angle);
      }

      return result.Composition((PolynomialTerm)cd.Last(), replacement);
    }

    public static Number ToSpherical(
      in this Number source, 
      in CartesianCoordinate cartesianPoint, 
      in HypersphericalCoordinate hypersphericalPoint)
    {
      return ToSpherical((PolynomialDivision)source, in cartesianPoint, in hypersphericalPoint);
    }

    public static HypersphericalCoordinateOnAxisView ToSphericalView(in this CartesianCoordinate p)
    {
      throw new NotImplementedException("Not yet supported");
    }

    public static PolynomialDivision Composition(in this Number source, in CartesianCoordinate c1, in CartesianCoordinate c2)
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
