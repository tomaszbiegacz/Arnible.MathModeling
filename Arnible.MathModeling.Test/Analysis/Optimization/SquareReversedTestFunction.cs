namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// 3 - (x-1)^2
  /// </summary>
  public class SquareReversedTestFunction : INumberFunctionWithDerivative
  {
    private static readonly Number _minValue = -100;
    private static readonly Number _maxValue = 100;
      
    public ref readonly Number MinX => ref _minValue;
    public ref readonly Number MaxX => ref _maxValue;
    
    public NumberFunctionPointWithDerivative ValueWithDerivative(in Number x)
    {
      return new NumberFunctionPointWithDerivative(
        x: x,
        y: 3 - (x - 1).ToPower(2),
        first: -2*(x-1)
        );
    }
  }
}