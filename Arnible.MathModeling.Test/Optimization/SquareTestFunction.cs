namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// (x-1)^2 + 3
  /// </summary>
  public class SquareTestFunction : INumberFunctionWithDerivative
  {
    private static readonly Number _minValue = -100;
    private static readonly Number _maxValue = 100;
      
    public ref readonly Number MinValue => ref _minValue;
    public ref readonly Number MaxValue => ref _maxValue;
    
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