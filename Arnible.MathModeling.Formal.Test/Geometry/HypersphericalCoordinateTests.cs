using Arnible.MathModeling.Algebra;
using System;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateTests
  {
    private static PolynomialDivision GetSum(uint inputCount)
    {
      var cartesianInputs = Number.Terms(inputCount);
      var cartesianPoint = new CartesianCoordinate(cartesianInputs.ToVector());
      var sphericalPoint = new HypersphericalCoordinate((PolynomialTerm)'R', Number.GreekTerms(inputCount - 1).ToVector());

      Number product = cartesianInputs.SumDefensive();
      return product.ToSpherical(cartesianPoint, sphericalPoint);
    }

    [Fact]
    public void GeneralizationForSum()
    {
      PolynomialDivision last = default;
      for (uint i = 2; i <= 5; ++i)
      {
        PolynomialDivision current = GetSum(i);
        if (last != default)
        {
          Assert.Equal(last, current.Composition(Number.GreekTerm(i - 2), 0));
        }
        last = current;
      }
    }

    private static void EqualityAfterTranformation(PolynomialDivision polynomial)
    {
      var cartesianPoint = new CartesianCoordinate(new NumberVector(Term.x, Term.y, Term.z));
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new NumberVector(Term.θ, Term.φ));

      PolynomialDivision sphericalPolynomial = polynomial.ToSpherical(cartesianPoint, sphericalPoint);

      double sqrt2 = Math.Sqrt(2);
      double sqrt3 = Math.Sqrt(3);
      double polynomialResult = polynomial.GetOperation(Term.x, Term.y, Term.z).Value(sqrt2, sqrt2, 2 * sqrt3);
      double sphericalResult = sphericalPolynomial.GetOperation(Term.r, Term.θ, Term.φ).Value(4, Math.PI / 4, Math.PI / 3);
      Assert.Equal(polynomialResult, sphericalResult, 10);
    }

    [Fact]
    public void EqualityAfterTranformation_Linear() => EqualityAfterTranformation(Term.x + Term.y + Term.z);

    [Fact]
    public void EqualityAfterTranformation_Polynomial3d() => EqualityAfterTranformation(1 + Term.x + 2 * Term.y * Term.z);    
  }
}
