namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// (x-1)^3 + 3 
  /// </summary>
  public class CubeTestFunction
  {
    public ValueWithDerivative1 ValueWithDerivative(in Number x)
    {
      return new ValueWithDerivative1(
        x: x,
        y: (x - 1).ToPower(3) + 3,
        first: 3*(x-1).ToPower(2)
      );
    }
  }
}