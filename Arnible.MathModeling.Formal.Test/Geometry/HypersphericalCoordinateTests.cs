using Arnible.MathModeling.Algebra.Polynomials;
using Arnible.MathModeling.Analysis;
using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateTests
  {
    private static PolynomialDivision GetSum(uint inputCount)
    {
      var cartesianInputs = Number.Terms(inputCount).ToArray();
      Number[] cartesianPoint = cartesianInputs;
      var sphericalPoint = new HypersphericalCoordinate((PolynomialTerm)'R', Number.GreekTerms(inputCount - 1).ToArray());

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
          last.AssertIsEqualTo(current.Composition(Number.GreekTerm(i - 2), 0));
        }
        last = current;
      }
    }

    private static void EqualityAfterTransformation(PolynomialDivision polynomial)
    {
      var cartesianPoint = new Number[] {Term.x, Term.y, Term.z};
      var sphericalPoint = new HypersphericalCoordinate(Term.r, new Number[] {Term.θ, Term.φ});

      PolynomialDivision sphericalPolynomial = polynomial.ToSpherical(cartesianPoint, sphericalPoint);

      double sqrt2 = Math.Sqrt(2);
      double sqrt3 = Math.Sqrt(3);
      Number polynomialResult = polynomial.GetOperation(Term.x, Term.y, Term.z).Value(sqrt2, sqrt2, 2 * sqrt3);
      double sphericalResult = sphericalPolynomial.GetOperation(Term.r, Term.θ, Term.φ).Value(4, Math.PI / 4, Math.PI / 3);
      polynomialResult.AssertIsEqualTo(sphericalResult);
    }

    [Fact]
    public void EqualityAfterTransformation_Linear() => EqualityAfterTransformation(Term.x + Term.y + Term.z);

    [Fact]
    public void EqualityAfterTransformation_Polynomial3d() => EqualityAfterTransformation(1 + Term.x + 2 * Term.y * Term.z);    
    
    [Fact]
    public void DerivativeByR()
    {
      var cartesianPoint = new Number[] {x, y, z};
      var sphericalPoint = new HypersphericalCoordinate(r, new Number[] {θ, φ});
      
      Span<Number> cartesianActual = new Number[3];
      sphericalPoint.ToCartesian(in cartesianActual);

      Span<Number> derivatives = new Number[cartesianPoint.Length];
      sphericalPoint.DerivativeByR(in derivatives);
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
      
      Span<Number> cartesianActual = new Number[5];
      sphericalPoint.ToCartesian(in cartesianActual);

      for (ushort anglePos = 0; anglePos < sphericalPoint.Angles.Length; ++anglePos)
      {
        PolynomialTerm angleTerm = (PolynomialTerm)sphericalPoint.Angles.Span[anglePos];
        var derivatives = new HypersphericalCoordinateOnAxisViewForAngleDerivatives(sphericalPoint, anglePos: anglePos).DerivativesCartesianVector;
        derivatives.Length.AssertIsEqualTo(sphericalPoint.DimensionsCount);
        for (ushort coordinatePos = 0; coordinatePos < cartesianActual.Length; ++coordinatePos)
        {
          PolynomialDivision coordinate = (PolynomialDivision)cartesianActual[coordinatePos];
          PolynomialDivision expected = coordinate.DerivativeBy(angleTerm);
          derivatives[coordinatePos].AssertIsEqualTo(expected);
        }
      }
    }
  }
}
