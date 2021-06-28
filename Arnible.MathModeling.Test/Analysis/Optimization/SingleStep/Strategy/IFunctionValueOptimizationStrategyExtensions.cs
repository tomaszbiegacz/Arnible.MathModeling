using System;
using Arnible.Export;
using Arnible.MathModeling.Geometry;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public static class IFunctionValueOptimizationStrategyExtensions
  {
    public static ushort FindOptimal(
      this IFunctionValueOptimizationStrategy strategy,
      ILoggerWithWriterFactory logger,
      ref FunctionMinimumImprovement solution)
    {
      logger
        .Write("Source parameters: ", solution.Parameters)
        .Write(" value: ", solution.Value)
        .NewLine();

      IRecordFileWriter serializer = logger.CreateTsvNotepad("function");
      try
      {
        ushort pos = 0;
        using(var writer = serializer.OpenRecord())
          solution.SerializeCurrentState(writer.FieldSerializer, 
            pos: pos, totalComplexity: 0, withExtendedSearch: false);
        
        Span<Number> currentParameters = stackalloc Number[solution.ParametersCount];
        bool isTheEnd = false;
        while(!isTheEnd)
        {
          pos++;
          solution.Parameters.CopyTo(currentParameters);

          logger
            .NewLine()
            .Write("Loop ", pos).NewLine()
            .Write("====").NewLine();
          var statistics = strategy.FindImprovedArguments(ref solution);
          
          using(var writer = serializer.OpenRecord())
            solution.SerializeCurrentState(writer.FieldSerializer, 
              pos: pos, 
              totalComplexity: statistics.Complexity, 
              withExtendedSearch: statistics.WithExtendedSearch);
        
          if(solution.Parameters.SequenceEqual(currentParameters))
          {
            logger.Write("I got stuck").NewLine();
            isTheEnd = true;
          } else if(solution.Function.IsOptimum(solution.Parameters))
          {
            logger.Write("I have found optimum").NewLine();
            isTheEnd = true;
          }
        }
        return pos;  
      }
      finally
      {
        // for tests this is sufficient
        serializer.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      }
    }
  }
}