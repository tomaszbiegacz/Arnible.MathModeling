using System;

namespace Arnible.MathModeling
{
  public static class AssertFormal
  {
    public static void Equal(PolynomialDivision p1, PolynomialDivision p2)
    {
      if (p1 != p2)
      {
        throw new InvalidOperationException($"Expected [{p2}], got [{p1}]");
      }
    }

    public static void VerifyDerivatives(PolynomialDivision value, Number term, IDerivative2 derivative)
    {
      VerifyDerivativesInternal(value, (PolynomialTerm)term, derivative);
    }

    private static void VerifyDerivativesInternal(PolynomialDivision value, PolynomialTerm term, IDerivative2 derivative)
    {
      var firstDerivative = value.DerivativeBy(term);
      Equal(firstDerivative, derivative.First);
      Equal(firstDerivative.DerivativeBy(term), derivative.Second);
    }
  }
}
