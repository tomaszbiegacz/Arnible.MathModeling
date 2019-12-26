namespace Arnible.MathModeling
{
  public static class MetaMath
  {
    public static PolynomialTerm Sin(PolynomialTerm term) => IndeterminateExpression.Sin((char)term);

    public static PolynomialTerm Cos(PolynomialTerm term) => IndeterminateExpression.Cos((char)term);

    public static Number Sin(Number a) => NumberMath.Sin(a);

    public static Number Cos(Number a) => NumberMath.Cos(a);
  }
}
