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
  [Collection(nameof(RosenbrockGeneralisationTests))]
  public class RosenbrockGeneralisationTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public RosenbrockGeneralisationTests(ITestOutputHelper output)
      : base(output)
    {
      _function = new RosenbrockGeneralisationTestFunction();
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Optimum(ushort dimensionsCount)
    {
      Span<Number> solutionParameters = stackalloc Number[dimensionsCount];
      solutionParameters.Fill(1);
      
      Span<Number> solutionBuffer = stackalloc Number[dimensionsCount];
      FunctionMinimumImprovement solution = new(
        _function, 
        solutionParameters, 
        in solutionBuffer);
      solution.Value.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    [Theory]
    [InlineData(2, 4, 0.2, 0.3, 2074, 5, false)]
    [InlineData(2, 4, 0.5, 0.3, 2074, 5, false)]
    [InlineData(3, 4, 0.2, 0.2, 6217, 5, false)]
    [InlineData(3, 4, 0.2, 0.3, 4150, 5, false)]
    [InlineData(3, 4, 0.2, 0.5, 4547, 5, false)]
    [InlineData(3, 8, 0.2, 0.3, 4150, 5, false)]
    [InlineData(4, 4, 0.3, 0.3, 6821, 5, false)]
    [InlineData(4, 8, 0.3, 0.3, 6821, 5, false)]
    [InlineData(5, 4, 0.2, 0.3, 9013, 5, false)]
    [InlineData(5, 4, 0.5, 0.3, 9013, 5, false)]
    [InlineData(6, 4, 0.5, 0.3, 13439, 4, false)]
    [InlineData(7, 4, 0.2, 0.3, 13252, 4, false)]
    [InlineData(8, 4, 0.2, 0.3, 11457, 4, false)]
    [InlineData(8, 4, 0.5, 0.3, 11457, 4, false)]
    [InlineData(8, 4, 0.8, 0.3, 11457, 4, false)]
    public void Xp_Yp_ExtendedUniformSearchDirection(
      ushort dimensionsCount,
      ushort conjugateDirectionBuffer,
      double extendedUniformSearchDirectionDisablingRatio,
      double extendedUniformSearchDirectionMinimumGradientRatio,
      ushort iterationsCount,
      ushort precision,
      bool saveLogsToFile)
    {
      if(saveLogsToFile)
      {
        BackupLogsToFile();
      }
      Span<Number> parameters = stackalloc Number[dimensionsCount];
      parameters.Fill(1.5);
      
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
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = extendedUniformSearchDirectionDisablingRatio,
        ExtendedUniformSearchDirectionMinimumGradientRatio = extendedUniformSearchDirectionMinimumGradientRatio
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      
      for(ushort i=0; i<dimensionsCount; ++i)
      {
        Assert.Equal(1, (double)solution.Parameters[i], precision);  
      }
      iterations.AssertIsEqualTo(iterationsCount);
    }
  }
}