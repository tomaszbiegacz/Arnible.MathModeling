using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test.Functions;
using Arnible.MathModeling.Geometry;
using Arnible.MathModeling.Test;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  [Collection(nameof(RosenbrockGeneralisationTestFunctionTests))]
  public class RosenbrockGeneralisationTestFunctionTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public RosenbrockGeneralisationTestFunctionTests(ITestOutputHelper output)
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
    [InlineData(2, 4, 2074, 5)]
    [InlineData(3, 4, 4150, 5)]
    [InlineData(3, 8, 4150, 5)]
    [InlineData(4, 4, 6821, 5)]
    [InlineData(4, 8, 6821, 5)]
    [InlineData(5, 4, 9013, 5)]
    [InlineData(6, 4, 13439, 4)]
    [InlineData(7, 4, 13252, 4)]
    [InlineData(8, 4, 11457, 4)]
    public void Xp_Yp_ExtendedUniformSearchDirection(
      ushort dimensionsCount,
      ushort conjugateDirectionBuffer,
      ushort iterationsCount,
      ushort precision)
    {
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
        ExtendedUniformSearchDirectionDisablingRatio = 0.5
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