using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Algebra.Polynomials;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;
using static Arnible.MathModeling.Algebra.Polynomials.Term;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnAxisViewTests
  {
    [Fact]
    public void DerivativeByR()
    {
      var cartesianPoint = new Number[] {x, y, z};
      var sphericalPoint = new HypersphericalCoordinate(r, new Number[] {θ, φ});
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      Span<Number> derivatives = new Number[cartesianPoint.Length];
      view.DerivativeByR(in derivatives);
      for (ushort dimensionPos = 0; dimensionPos < cartesianPoint.Length; ++dimensionPos)
      {
        var symbol = (PolynomialDivision)cartesianPoint[dimensionPos];
        (r * derivatives[dimensionPos]).AssertIsEqualTo(symbol.ToSpherical(cartesianPoint, sphericalPoint));
      }
    }

    [Fact]
    public void GetCartesianAxisViewsRatiosDerivativesByAngle()
    {
      var sphericalPoint = new HypersphericalCoordinate(r, new Number[] {α, β, γ, δ});
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      for (ushort anglePos = 0; anglePos < sphericalPoint.Angles.Length; ++anglePos)
      {
        PolynomialTerm angleTerm = (PolynomialTerm)sphericalPoint.Angles[anglePos];
        var derivatives = view.GetAngleDerivativesView(anglePos: anglePos).CartesianAxisViewsRatiosDerivatives.ToArray();
        IsEqualToExtensions.AssertIsEqualTo(derivatives.Length, view.DimensionsCount);
        for (ushort coordinatePos = 0; coordinatePos < view.Coordinates.Length; ++coordinatePos)
        {
          PolynomialDivision coordinate = (PolynomialDivision)view.Coordinates[coordinatePos];
          PolynomialDivision expected = coordinate.DerivativeBy(angleTerm);
          derivatives[coordinatePos].AssertIsEqualTo(expected);
        }
      }
    }

    [Fact]
    public void GetRectangularViewDerivativeByAngle()
    {
      var sphericalPoint = new HypersphericalCoordinate(r, new Number[] {α, β, γ});
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      HypersphericalCoordinateOnRectangularViewWithDerivative recView = view
        .GetAngleDerivativesView(anglePos: 2)
        .GetRectangularViewDerivativeByAngle(axisA: 0, axisB: 2);
      IsEqualToExtensions.AssertIsEqualTo(r, recView.R);

      IsEqualToExtensions.AssertIsEqualTo(Cos(γ) * Cos(β) * Cos(α), recView.RatioX);
      IsEqualToExtensions.AssertIsEqualTo(r * Cos(γ) * Cos(β) * Cos(α), recView.X);
      IsEqualToExtensions.AssertIsEqualTo(-1 * r * Sin(γ) * Cos(β) * Cos(α), recView.DerivativeForX.First);

      IsEqualToExtensions.AssertIsEqualTo(Sin(β) * Cos(γ), recView.RatioY);
      IsEqualToExtensions.AssertIsEqualTo(r * Sin(β) * Cos(γ), recView.Y);
      IsEqualToExtensions.AssertIsEqualTo(-1 * r * Sin(γ) * Sin(β), recView.DerivativeForY.First);
    }
  }
}
