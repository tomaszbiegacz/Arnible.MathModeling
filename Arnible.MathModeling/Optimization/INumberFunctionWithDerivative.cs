namespace Arnible.MathModeling.Optimization
{
  public interface INumberFunctionWithDerivative
  {
    NumberValueWithDerivative1 ValueWithDerivative(in Number x);
  }
}