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
  [Collection(nameof(RastriginTests))]
  public class RastriginTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public RastriginTests(ITestOutputHelper output)
      : base(output)
    {
      _function = new RastriginTestFunction();
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Optimum(ushort dimensionsCount)
    {
      Span<Number> solutionParameters = stackalloc Number[dimensionsCount];
      solutionParameters.Fill(0);
      
      Span<Number> solutionBuffer = stackalloc Number[dimensionsCount];
      FunctionMinimumImprovement solution = new(
        _function, 
        solutionParameters, 
        in solutionBuffer);
      
      solution.Value.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    [Theory]
    [InlineData(2, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(3, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(4, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(5, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(6, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(7, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(8, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(9, 4, 0.2, 0.3, 12, 8, false)]
    [InlineData(10, 4, 0.2, 0.3, 12, 8, false)]
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
      
      GoldenSecantStrategy strategy = new(-6, 6, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = extendedUniformSearchDirectionDisablingRatio,
        ExtendedUniformSearchDirectionMinimumGradientRatio = extendedUniformSearchDirectionMinimumGradientRatio
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      
      for(ushort i=0; i<dimensionsCount; ++i)
      {
        Assert.Equal(0, (double)solution.Parameters[i], precision);  
      }
      iterations.AssertIsEqualTo(iterationsCount);
    }
  }
}