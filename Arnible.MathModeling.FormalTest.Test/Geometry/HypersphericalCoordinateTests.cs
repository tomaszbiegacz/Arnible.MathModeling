using Arnible.MathModeling.Geometry;
using System;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.FormalTest.Test.Geometry
{
  public class HypersphericalCoordinateTests
  {
    private static Polynomial GetSum(uint inputCount)
    {
      var cartesianInputs = Number.Terms(inputCount);
      var cartesianPoint = new CartesianCoordinate(cartesianInputs);
      var sphericalPoint = new HypersphericalCoordinate((PolynomialTerm)'R', Number.GreekTerms(inputCount - 1));

      Polynomial product = cartesianInputs.Sum();
      return product.ToSpherical(cartesianPoint, sphericalPoint);
    }

    [Fact]
    public void GeneralizationForSum()
    {
      Polynomial last = default;
      for (uint i = 2; i <= 5; ++i)
      {
        Polynomial current = GetSum(i);
        if (last != default)
        {          
          Assert.Equal(last, current.Composition(Number.GreekTerm(i - 2), Math.PI / 2));
        }
        last = current;
      }
    }

    private void EqualityAfterTranformation(Polynomial polynomial)
    {
      var cartesianPoint = new CartesianCoordinate(new Number[] { Term.x, Term.y, Term.z });
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new Number[] { Term.θ, Term.φ });
      
      var sphericalPolynomial = polynomial.ToSpherical(cartesianPoint, sphericalPoint);

      double sqrt2 = Math.Sqrt(2);
      double sqrt3 = Math.Sqrt(3);
      double polynomialResult = polynomial.GetOperation(Term.x, Term.y, Term.z).Value(sqrt2, sqrt2, 2 * sqrt3);
      double sphericalResult = sphericalPolynomial.GetOperation(Term.r, Term.θ, Term.φ).Value(4, Math.PI / 4, Math.PI / 6);
      Assert.Equal(polynomialResult, sphericalResult, 10);
    }

    [Fact]
    public void EqualityAfterTranformation_Linear() => EqualityAfterTranformation(Term.x + Term.y + Term.z);

    [Fact]
    public void EqualityAfterTranformation_Polynomial3d() => EqualityAfterTranformation(1 + Term.x + 2 * Term.y * Term.z);
  }
}
