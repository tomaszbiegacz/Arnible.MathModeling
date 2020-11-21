namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// 3 - (x-1)^2
  /// </summary>
  public class SquareReversedTestFunction : INumberFunctionWithDerivative
  {
    public NumberValueWithDerivative1 ValueWithDerivative(in Number x)
    {
      return new NumberValueWithDerivative1(
        x: x,
        y: 3 - (x - 1).ToPower(2),
        first: -2*(x-1)
        );
    }
  }
}