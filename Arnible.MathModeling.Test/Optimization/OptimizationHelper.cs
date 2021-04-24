using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization;

namespace Arnible.MathModeling.Optimization.Test
{
  public static class OptimizationHelper
  {
    public static ushort FindOptimal(IUnimodalOptimization method)
    {
      Number width = method.Width;
      Number value = method.Y;

      ushort i = 0;
      while(!method.IsOptimal)
      {
        i++;
        method.MoveNext().AssertIsTrue();
        method.Width.AssertIsLessThan(width);
        method.Y.AssertIsLessEqualThan(value);

        width = method.Width;
        value = method.Y;
      }

      return i;
    }
  }
}