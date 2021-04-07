using Arnible.MathModeling.Analysis.Optimization;

namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// 3 - (x-1)^2
  /// </summary>
  public class SquareReversedTestFunction : INumberFunctionWithDerivative
  {
    private static readonly Number _minValue = -100;
    private static readonly Number _maxValue = 100;
      
    public ref readonly Number MinValue => ref _minValue;
    public ref readonly Number MaxValue => ref _maxValue;
    
    public FunctionPointWithDerivative ValueWithDerivative(in Number x)
    {
      return new FunctionPointWithDerivative(
        x: x,
        y: 3 - (x - 1).ToPower(2),
        first: -2*(x-1)
        );
    }
  }
}