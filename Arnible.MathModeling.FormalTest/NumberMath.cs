namespace Arnible.MathModeling
{
  public static class NumberMath
  {
    public static Number Sin(Number a) => (PolynomialTerm)IndeterminateExpression.Sin((PolynomialTerm)a);

    public static Number Cos(Number a) => (PolynomialTerm)IndeterminateExpression.Cos((PolynomialTerm)a);
  }
}
