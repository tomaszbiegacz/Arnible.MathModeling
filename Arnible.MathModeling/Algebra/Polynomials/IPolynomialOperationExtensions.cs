using Arnible.MathModeling.Analysis;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  public static class IPolynomialOperationExtensions
  {
    public static IFinitaryOperation<double> GetOperation(
      this IndeterminateExpression operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables,
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this Polynomial operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables,
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this PolynomialDivision operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables,
        valueCalculation: x => operation.Value(x));
    }
    
    public static IFinitaryOperation<double> GetOperation(
      this PolynomialTerm operation,
      params PolynomialTerm[] variables)
    {
      return new PolynomialFinitaryOperation(
        variables: variables,
        valueCalculation: x => operation.Value(x));
    }
  }
}
