namespace Arnible.MathModeling
{
  public static class NumberMath
  {
    public static Number Sin(Number a) => PolynomialTerm.Sin((PolynomialTerm)a);

    public static Number Cos(Number a) => PolynomialTerm.Cos((PolynomialTerm)a);
  }
}
