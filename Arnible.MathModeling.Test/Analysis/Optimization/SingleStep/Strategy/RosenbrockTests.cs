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
      solution.SourceValue.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
    
    //
    // splot [-2:2] [-2:2] (1-x)**2 + 100*(y - x**2)**2 with pm3d
    // splot [0.7:1.7] [0.7:1.7] (1-x)**2 + 100*(y - x**2)**2 with pm3d
    //
    
    [Fact(Skip = "I got stuck")]
    public void Xp_Yn_Wide()
    {
      BackupLogsToFile();
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1.5, -1.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        WideSearch = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(1, (double)solution.Parameters[0], 2);
      Assert.Equal(1, (double)solution.Parameters[1], 2);
      iterations.AssertIsEqualTo(48);
    }
    
    [Fact(Skip = "sensitive to correlated variables")]
    public void Xp_Yn_UniformDirection()
    {
      BackupLogsToFile();
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1.5, -1.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        UniformSearchDirection = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(1.11, (double)solution.Parameters[0], 2);
      Assert.Equal(1.24, (double)solution.Parameters[1], 2);
      iterations.AssertIsEqualTo(48);
      
      // Parameters: [1.361087917365479,1.852560318798297]
      // Gradient: [0.7221758347309581,0]
      // plot [-1:1] (1-1.361087917365479)**2 + 100*(1.85256031879829 + y - 1.361087917365479**2)**2
      // p1*p1 = p2
    }
    
    [Fact(Skip = "stack at border")]
    public void Xp_Yp_UniformDirection()
    {
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        new Number[] { 1.5, 1.5 }, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-2, 2, Logger)
      {
        UniformSearchDirection = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(48);
    }
    
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
        ExtendedUniformSearchDirection = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(6099);
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
        ExtendedUniformSearchDirection = true
      };
      ushort iterations = strategy.FindOptimal(Logger, in solution);
      Assert.Equal(1, (double)solution.Parameters[0], 5);
      Assert.Equal(1, (double)solution.Parameters[1], 5);
      iterations.AssertIsEqualTo(5849);
    }
  }
}