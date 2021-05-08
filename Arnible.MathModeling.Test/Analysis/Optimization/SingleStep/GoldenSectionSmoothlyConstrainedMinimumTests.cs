using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test
{
  public class GoldenSectionSmoothlyConstrainedMinimumTests : TestsWithLogger
  {
    private readonly ISingleStepOptimization _optimizer;
    
    public GoldenSectionSmoothlyConstrainedMinimumTests(ITestOutputHelper output) : base(output)
    {
      _optimizer = new GoldenSectionSmoothlyConstrainedMinimum(Logger);
    }
    
    /// <summary>
    /// Use secant to find optimum in one step
    /// </summary>
    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(3);

      Number actual = _optimizer.Optimize(f, in a, b.X);
      actual.AssertIsEqualTo(1);
    }
    
    [Fact]
    public void Unimodal_SquareReversed_Maximum()
    {
      var f = new SquareReversedTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(3);
      
      try
      {
        _optimizer.Optimize(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(NotAbleToOptimizeException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void Unimodal_Square_WrongDirection_PositiveDerivative()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);

      try
      {
        _optimizer.Optimize(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(NotAbleToOptimizeException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void Unimodal_Square_WrongDirection_NegativeDerivative()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(0.5);
      var b = f.ValueWithDerivative(-2);

      try
      {
        _optimizer.Optimize(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(NotAbleToOptimizeException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void Unimodal_Square_AtOptimum()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(2);
      
      try
      {
        _optimizer.Optimize(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(NotAbleToOptimizeException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void Unimodal_Sin_Optimum()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1.3 * Math.PI);
      var b = f.ValueWithDerivative(0.4 * Math.PI);
      
      double actual = (double)_optimizer.Optimize(f, in a, b.X);
      Assert.Equal(-0.5 * Math.PI, actual, precision: 1);
    }
    
    [Fact]
    public void Multimodal_Sin()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1.1 * Math.PI);
      var b = f.ValueWithDerivative(2 * Math.PI);
      
      a.First.AssertIsLessThan(0);
      b.First.AssertIsGreaterThan(0);
      
      double actual = (double)_optimizer.Optimize(f, in a, b.X);
      Assert.Equal(-0.5 * Math.PI, actual, precision: 1);
    }
  }
}