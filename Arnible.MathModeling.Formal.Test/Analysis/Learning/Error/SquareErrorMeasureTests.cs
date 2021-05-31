using Arnible.Assertions;
using Arnible.MathModeling.Algebra.Polynomials;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;

namespace Arnible.MathModeling.Analysis.Learning.Error.Test
{
  public class SquareErrorMeasureTests
  {
    static void AreDerivativesEqual(in PolynomialDivision value, in Number term, in Derivative1Value actual)
    {
      PolynomialTerm termSingle = (PolynomialTerm)term;
      PolynomialDivision firstDerivative = value.DerivativeBy(termSingle);
      actual.First.AssertIsEqualTo(firstDerivative);
    }
    
    [Fact]
    public void VerifyDerivatives()
    {
      IErrorMeasureSupervisedLearning<Number> errorMeasure = new SquareErrorMeasure();
      PolynomialDivision errorValue = (PolynomialDivision)errorMeasure.ErrorValue(x, y);
      AreDerivativesEqual(errorValue, y, errorMeasure.ErrorDerivativeByActual(expected: x, actual: y));
    }
  }
}
