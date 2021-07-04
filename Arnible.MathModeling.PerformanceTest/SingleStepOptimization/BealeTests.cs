using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization;
using Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy;
using Arnible.MathModeling.Analysis.Optimization.Test.Functions;
using Arnible.MathModeling.Geometry;
using Arnible.MathModeling.Test;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.PerformanceTest.SingleStepOptimization
{
  [Collection(nameof(BealeTests))]
  public class BealeTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public BealeTests(ITestOutputHelper output)
      : base(output)
    {
      _function = new BealeTestFunction();
    }
    
    [Theory]
    [InlineData(4, 0.2, 0.3, 117, 7, false)]
    [InlineData(6, 0.2, 0.3, 117, 7, false)]
    [InlineData(4, 0.1, 0.3, 117, 7, false)]
    [InlineData(4, 0.6, 0.3, 117, 7, false)]
    [InlineData(4, 0.2, 0.7, 117, 7, false)]
    public void Xp_Yp(
      ushort conjugateDirectionBuffer,
      double? extendedUniformSearchDirectionDisablingRatio,
      double extendedUniformSearchDirectionMinimumGradientRatio,
      ushort iterationsCount,
      ushort precision,
      bool saveLogsToFile)
    {
      if(saveLogsToFile)
      {
        BackupLogsToFile();
      }
      ushort dimensionsCount = 2;
      Span<Number> parameters = stackalloc Number[dimensionsCount];
      parameters.Fill(4);
      
      ConjugateDirection direction = new(
        new Span2D<Number>(
          stackalloc Number[dimensionsCount * conjugateDirectionBuffer], 
          dimensionsCount),
        new SpanSingle<ushort>(stackalloc ushort[1]),
        new SpanSingle<bool>(stackalloc bool[1]));
      
      Span<Number> solutionBuffer = stackalloc Number[dimensionsCount];
      FunctionMinimumImprovement solution = new(
        _function, 
        parameters, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-4.5, 4.5, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = extendedUniformSearchDirectionDisablingRatio,
        ExtendedUniformSearchDirectionMinimumGradientRatio = extendedUniformSearchDirectionMinimumGradientRatio
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      
      Assert.Equal(3, (double)solution.Parameters[0], precision);
      Assert.Equal(0.5, (double)solution.Parameters[1], precision);
      
      iterations.AssertIsEqualTo(iterationsCount);
    }
  }
}