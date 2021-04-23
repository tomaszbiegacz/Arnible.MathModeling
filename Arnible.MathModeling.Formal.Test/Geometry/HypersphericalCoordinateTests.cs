using Arnible.MathModeling.Algebra.Polynomials;
using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateTests
  {
    private static PolynomialDivision GetSum(uint inputCount)
    {
      var cartesianInputs = Number.Terms(inputCount).ToVector();
      CartesianCoordinate cartesianPoint = cartesianInputs;
      var sphericalPoint = new HypersphericalCoordinate((PolynomialTerm)'R', Number.GreekTerms(inputCount - 1).ToAngleVector());

      Number product = cartesianInputs.SumDefensive();
      return (PolynomialDivision)product.ToSpherical(cartesianPoint, sphericalPoint);
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
          IsEqualToExtensions.AssertIsEqualTo(last, current.Composition(Number.GreekTerm(i - 2), 0));
        }
        last = current;
      }
    }

    private static void EqualityAfterTransformation(PolynomialDivision polynomial)
    {
      CartesianCoordinate cartesianPoint = new NumberVector(Term.x, Term.y, Term.z);
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new HypersphericalAngleVector(Term.θ, Term.φ));

      PolynomialDivision sphericalPolynomial = polynomial.ToSpherical(cartesianPoint, sphericalPoint);

      double sqrt2 = Math.Sqrt(2);
      double sqrt3 = Math.Sqrt(3);
      double polynomialResult = polynomial.GetOperation(Term.x, Term.y, Term.z).Value(sqrt2, sqrt2, 2 * sqrt3);
      double sphericalResult = sphericalPolynomial.GetOperation(Term.r, Term.θ, Term.φ).Value(4, Math.PI / 4, Math.PI / 3);
      IsEqualToExtensions.AssertIsEqualTo(polynomialResult, sphericalResult);
    }

    [Fact]
    public void EqualityAfterTransformation_Linear() => EqualityAfterTransformation(Term.x + Term.y + Term.z);

    [Fact]
    public void EqualityAfterTransformation_Polynomial3d() => EqualityAfterTransformation(1 + Term.x + 2 * Term.y * Term.z);    
  }
}
