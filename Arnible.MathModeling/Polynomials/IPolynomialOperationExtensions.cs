namespace Arnible.MathModeling.Polynomials
{
  public static class IPolynomialOperationExtensions
  {
    public static IFinitaryOperation<double> GetOperation(
      this IndeterminateExpression operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables.Select(pt => (char)pt),
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this Polynomial operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables.Select(pt => (char)pt),
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this PolynomialDivision operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables.Select(pt => (char)pt),
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this PolynomialTerm operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables.Select(pt => (char)pt),
        valueCalculation: x => operation.Value(x));
    }
  }
}
