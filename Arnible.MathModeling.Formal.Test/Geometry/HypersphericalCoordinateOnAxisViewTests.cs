using Arnible.MathModeling.Algebra;
using Xunit;
using static Arnible.MathModeling.Term;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnAxisViewTests
  {
    [Fact]
    public void DerivativeByR()
    {
      var cartesianPoint = new CartesianCoordinate(new NumberVector(x, y, z));
      var sphericalPoint = new HypersphericalCoordinate(r, new NumberVector(θ, φ));
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      var derivatives = view.DerivativeByR().ToArray();      
      Assert.Equal((int)cartesianPoint.DimensionsCount, derivatives.Length);
      for (uint dimensionPos = 0; dimensionPos < cartesianPoint.DimensionsCount; ++dimensionPos)
      {
        var symbol = (PolynomialDivision)cartesianPoint.Coordinates[dimensionPos];
        AssertFormal.Equal(symbol.ToSpherical(cartesianPoint, sphericalPoint), r * derivatives[dimensionPos].First);
      }
    }

    [Fact]
    public void GetCartesianAxisViewsRatiosDerivativesByAngle()
    {
      var sphericalPoint = new HypersphericalCoordinate(r, new NumberVector(α, β, γ, δ));      
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      for(uint anglePos = 0; anglePos < sphericalPoint.Angles.Length; ++anglePos)
      {
        PolynomialTerm angleTerm = (PolynomialTerm)sphericalPoint.Angles[anglePos];
        var derivatives = view.GetCartesianAxisViewsRatiosDerivativesByAngle(anglePos).ToArray();
        Assert.Equal(view.DimensionsCount, (uint)derivatives.Length);
        for(uint coordinatePos = 0; coordinatePos < view.Coordinates.Length; ++coordinatePos)
        {
          PolynomialDivision coordinate = view.Coordinates[coordinatePos];
          PolynomialDivision expected = coordinate.DerivativeBy(angleTerm) / r;
          Assert.Equal<PolynomialDivision>(expected, derivatives[coordinatePos].First);
        }
      }      
    }

    [Fact]
    public void GetRectangularViewDerivativeByAngle()
    {
      var sphericalPoint = new HypersphericalCoordinate(r, new NumberVector(α, β, γ));
      HypersphericalCoordinateOnAxisView view = sphericalPoint.ToCartesianView();

      HypersphericalCoordinateOnRectangularViewWithDerivative recView = view.GetRectangularViewDerivativeByAngle(axisA: 0, axisB: 2, anglePos: 2);
      Assert.Equal(r, recView.R);

      Assert.Equal(Cos(γ) * Cos(β) * Cos(α), recView.RatioX);
      Assert.Equal(r * Cos(γ) * Cos(β) * Cos(α), recView.X);
      Assert.Equal(-1 * Sin(γ) * Cos(β) * Cos(α), recView.RatioXDerivative.First);

      Assert.Equal(Sin(β)*Cos(γ), recView.RatioY);
      Assert.Equal(r * Sin(β)*Cos(γ), recView.Y);
      Assert.Equal(-1 * Sin(γ) * Sin(β), recView.RatioYDerivative.First);
    }
  }
}
