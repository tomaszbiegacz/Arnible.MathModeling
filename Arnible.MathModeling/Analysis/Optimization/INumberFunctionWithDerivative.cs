namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    ref readonly Number MinX { get; }
    ref readonly Number MaxX { get; }
    
    NumberFunctionPointWithDerivative ValueWithDerivative(in Number x);
  }
}