namespace Arnible.MathModeling.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    FunctionPointWithDerivative ValueWithDerivative(in Number x);
  }
}