using System;
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
        Span<Number> currentParameters = stackalloc Number[solution.Parameters.Length];
        bool isTheEnd = false;
        while(!isTheEnd)
        {
          solution.Parameters.CopyTo(currentParameters);
          
          pos++;
          logger.NewLine().Write("Loop ", pos).NewLine();
          strategy.FindImprovedArguments(in solution);
          
          isTheEnd = solution.Parameters.SequenceEqual(currentParameters)
                     || solution.Function.IsOptimum(solution.Parameters);
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