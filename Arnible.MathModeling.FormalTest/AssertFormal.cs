using System;

namespace Arnible.MathModeling
{
  public static class AssertFormal
  {
    public static void Equal(Polynomial p1, Polynomial p2)
    {
      if (p1 != p2)
      {
        throw new InvalidOperationException($"Expected [{p2}], got [{p1}]");
      }
    }

    public static void VerifyDerivatives(Polynomial value, Number term, IDerivative2 derivative) => VerifyDerivatives(value, (PolynomialTerm)term, derivative);

    public static void VerifyDerivatives(Polynomial value, PolynomialTerm term, IDerivative2 derivative)
    {
      var firstDerivative = value.DerivativeBy(term);
      Equal(firstDerivative, derivative.First);
      Equal(firstDerivative.DerivativeBy(term), derivative.Second);
    }
  }
}
