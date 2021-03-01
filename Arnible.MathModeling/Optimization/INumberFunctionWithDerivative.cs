namespace Arnible.MathModeling.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    ref readonly Number MinValue { get; }
    ref readonly Number MaxValue { get; }
    
    FunctionPointWithDerivative ValueWithDerivative(in Number x);
  }
}