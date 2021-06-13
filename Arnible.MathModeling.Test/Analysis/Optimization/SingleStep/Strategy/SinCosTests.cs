using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public class SinTests
  {
    private readonly IFunctionValueAnalysis _function = new SinCos2DTestFunction();
    private readonly XunitLogger _logger;
    
    public SinTests(ITestOutputHelper output)
    {
      _logger = new XunitLogger(output);
    }
    
    [Fact]
    public void Optimum()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { -0.5 * Math.PI, -1 * Math.PI }, 
        in solutionBuffer);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    [Fact]
    public void Multimodal_GoldenSecant()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { -1.1 * Math.PI, -1.1 * Math.PI }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2 * Math.PI, 2 * Math.PI, _logger);
      ushort iterations = strategy.FindOptimal(_logger, in solution);
      Assert.Equal(-0.5 * Math.PI, (double)solution.Parameters[0], 8);
      Assert.Equal(-1 * Math.PI, (double)solution.Parameters[1], 8);
      iterations.AssertIsEqualTo(15);
    }
  }
}