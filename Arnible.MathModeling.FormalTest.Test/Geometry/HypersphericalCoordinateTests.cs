using Arnible.MathModeling.Geometry;
using System;
using Arnible.MathModeling.Algebra;
using Xunit;
using System.Linq;

namespace Arnible.MathModeling.FormalTest.Test.Geometry
{
  public class HypersphericalCoordinateTests
  {
    private static Polynomial GetSum(uint inputCount)
    {
      var cartesianInputs = Number.Terms(inputCount);
      var cartesianPoint = new CartesianCoordinate(cartesianInputs.ToVector());
      var sphericalPoint = new HypersphericalCoordinate((PolynomialTerm)'R', Number.GreekTerms(inputCount - 1).ToVector());

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
          Assert.Equal(last, current.Composition(Number.GreekTerm(i - 2), 0));
        }
        last = current;
      }
    }

    private static void EqualityAfterTranformation(Polynomial polynomial)
    {
      var cartesianPoint = new CartesianCoordinate(new NumberVector(Term.x, Term.y, Term.z));
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new NumberVector(Term.θ, Term.φ));
      
      var sphericalPolynomial = polynomial.ToSpherical(cartesianPoint, sphericalPoint);

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

    [Fact]
    public void DerivativeByRForCartesianCoordinates()
    {
      var cartesianPoint = new CartesianCoordinate(new NumberVector(Term.x, Term.y, Term.z));
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new NumberVector(Term.θ, Term.φ));

      var derivatives = sphericalPoint.DerivativeByRForCartesianCoordinates().ToArray();
      Number r = Term.r;

      Assert.Equal((int)cartesianPoint.DimensionsCount, derivatives.Length);
      for(uint i=0; i<cartesianPoint.DimensionsCount; ++i)
      {
        var symbol = (Polynomial)cartesianPoint.Coordinates[i];
        AssertFormal.Equal(symbol.ToSpherical(cartesianPoint, sphericalPoint), r * derivatives[i].First);
      }
    }
  }
}
