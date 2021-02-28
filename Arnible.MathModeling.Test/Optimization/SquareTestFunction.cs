namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// (x-1)^2 + 3
  /// </summary>
  public class SquareTestFunction : INumberFunctionWithDerivative
  {
    public Number MinValue => -100;
    public Number MaxValue => 100;
    
    public FunctionPointWithDerivative ValueWithDerivative(in Number x)
    {
      return new FunctionPointWithDerivative(
        x: x,
        y: (x - 1).ToPower(2) + 3,
        first: 2*(x-1)
        );
    }
  }
}