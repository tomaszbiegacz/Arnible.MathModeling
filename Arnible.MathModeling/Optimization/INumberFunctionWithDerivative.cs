namespace Arnible.MathModeling.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    ValueWithDerivative1 ValueWithDerivative(in Number x);
  }
}