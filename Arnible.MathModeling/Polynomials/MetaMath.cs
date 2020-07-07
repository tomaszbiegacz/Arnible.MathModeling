namespace Arnible.MathModeling.Polynomials
{
  public static class MetaMath
  {
    public static PolynomialTerm Sin(PolynomialTerm term) => IndeterminateExpression.Sin((char)term);

    public static PolynomialTerm Cos(PolynomialTerm term) => IndeterminateExpression.Cos((char)term);    
  }
}
