namespace Arnible.MathModeling.Polynomials
{
  public static class IPolynomialOperationExtensions
  {
    public static IFinitaryOperation<double> GetOperation(
      this IPolynomialOperation operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(operation, variables.Select(pt => (char)pt));
    }
  }
}
