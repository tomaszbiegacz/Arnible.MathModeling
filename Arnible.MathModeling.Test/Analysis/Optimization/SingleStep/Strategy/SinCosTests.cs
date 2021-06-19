using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test;
using Arnible.MathModeling.Test;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public class SinTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public SinTests(ITestOutputHelper output)
    : base(output)
    {
      _function = new SinCos2DTestFunction();
    }
    
    [Fact]
    public void Optimum()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { -0.5 * Math.PI, -1 * Math.PI }, 
        in solutionBuffer);
      solution.SourceValue.AssertIsEqualTo(-2);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    [Fact]
    public void Multimodal_GoldenSecant_1()
    {
      // splot [-6:6] [-6:6] sin(x) + cos(y) with pm3d
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { -4, -4 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-6, 6, Logger)
      {
        WideSearch = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(-0.5 * Math.PI, (double)solution.Parameters[0], 8);
      Assert.Equal(-1 * Math.PI, (double)solution.Parameters[1], 8);
      iterations.AssertIsEqualTo(8);
    }
    
    [Fact]
    public void Multimodal_GoldenSecant_2()
    {
      // splot [-6:6] [-6:6] sin(x) + cos(y) with pm3d
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { -3.456, -3.456 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-6, 6, Logger)
      {
        WideSearch = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(-0.5 * Math.PI, (double)solution.Parameters[0], 8);
      Assert.Equal(-1 * Math.PI, (double)solution.Parameters[1], 8);
      iterations.AssertIsEqualTo(11);
    }
  }
}