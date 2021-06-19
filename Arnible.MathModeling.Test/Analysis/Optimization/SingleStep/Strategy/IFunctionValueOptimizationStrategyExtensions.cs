using System;
using Arnible.Export;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public static class IFunctionValueOptimizationStrategyExtensions
  {
    public static ushort FindOptimal(
      this IFunctionValueOptimizationStrategy strategy,
      ILoggerWithWriterFactory logger,
      in FunctionMinimumImprovement solution)
    {
      logger
        .Write("Source parameters: ", solution.SourceParameters)
        .Write(" value: ", solution.SourceValue)
        .NewLine();
      
      IRecordFileWriter serializer = logger.CreateTsvNotepad("function");
      try
      {
        ushort pos = 0;
        Span<Number> currentParameters = stackalloc Number[solution.Parameters.Length];
        bool isTheEnd = false;
        while(!isTheEnd)
        {
          using(var writer = serializer.OpenRecord())
            solution.SerializeCurrentState(pos, writer.FieldSerializer);
        
          pos++;
          solution.Parameters.CopyTo(currentParameters);

          logger.NewLine().Write("Loop ", pos).NewLine();
          strategy.FindImprovedArguments(in solution);
        
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
        serializer.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      }
    }
  }
}