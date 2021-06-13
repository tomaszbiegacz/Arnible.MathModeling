using Arnible.Assertions;
using Arnible.Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public static class IFunctionValueOptimizationStrategyExtensions
  {
    public static ushort FindOptimal(
      this IFunctionValueOptimizationStrategy strategy,
      XunitLogger logger,
      in FunctionMinimumImprovement solution)
    {
      logger
        .Write("Source parameters: ", solution.SourceParameters)
        .Write(" value: ", solution.SourceValue)
        .NewLine();
      try
      {
        ushort pos = 0;
        while(!solution.Function.IsOptimum(solution.Parameters))
        {
          pos++;
          logger.NewLine().Write("Loop ", pos).NewLine();
          strategy.FindImprovedArguments(in solution);
          solution.IsNewFound.AssertIsTrue();
        }
        return pos;  
      }
      finally
      {
        logger.Flush();
      }
    }
  }
}