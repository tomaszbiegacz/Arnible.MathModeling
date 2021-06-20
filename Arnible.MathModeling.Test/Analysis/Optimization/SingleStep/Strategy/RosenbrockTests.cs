using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test;
using Arnible.MathModeling.Test;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public class RosenbrockTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public RosenbrockTests(ITestOutputHelper output)
    : base(output)
    {
      _function = new RosenbrockTestFunction();
    }
    
    [Fact]
    public void Optimum()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1, 1 }, 
        in solutionBuffer);
      solution.Value.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    //
    // splot [-2:2] [-2:2] (1-x)**2 + 100*(y - x**2)**2 with pm3d
    // splot [0.7:1.7] [0.7:1.7] (1-x)**2 + 100*(y - x**2)**2 with pm3d
    //

    [Fact]
    public void Xp_Yp_ExtendedUniformSearchDirection()
    {
      BackupLogsToFile();
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1.5, 1.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = 0.5
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(2074);
    }
    
    [Fact]
    public void Xp_Yn_ExtendedUniformSearchDirection()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1.5, -1.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = 0.5
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(2305);
    }
    
    [Fact]
    public void Xp_Yp_ExtendedUniformSearchDirection_2()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 2.5, 2.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-3, 3, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = 0.5
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(3897);
    }
    
    [Fact]
    public void Xp_Yn_ExtendedUniformSearchDirection_2()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 2.5, -2.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-3, 3, Logger)
      {
        UniformSearchDirection = true,
        ExtendedUniformSearchDirectionDisablingRatio = 0.5
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(2996);
    }
  }
}