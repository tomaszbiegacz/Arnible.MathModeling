using Arnible.MathModeling.Polynomials;

namespace Arnible.MathModeling
{
  public static class NumberMath
  {
    public static Number Sin(in Number a) => PolynomialTerm.Sin((PolynomialTerm)a);

    public static Number Cos(in Number a) => PolynomialTerm.Cos((PolynomialTerm)a);
  }
}
