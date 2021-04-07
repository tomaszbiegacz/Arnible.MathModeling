using Arnible.MathModeling.Analysis.Optimization;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Optimization.Test
{
  public static class OptimizationHelper
  {
    public static uint FindOptimal(IUnimodalOptimization method)
    {
      Number width = method.Width;
      Number value = method.Y;

      uint i = 0;
      while(!method.IsOptimal)
      {
        i++;
        IsTrue(method.MoveNext());
        IsLowerThan(width, method.Width);
        IsLowerEqualThan(value, method.Y);

        width = method.Width;
        value = method.Y;
      }

      return i;
    }
  }
}