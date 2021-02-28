namespace Arnible.MathModeling.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    Number MinValue { get; }
    Number MaxValue { get; }
    
    FunctionPointWithDerivative ValueWithDerivative(in Number x);
  }
}