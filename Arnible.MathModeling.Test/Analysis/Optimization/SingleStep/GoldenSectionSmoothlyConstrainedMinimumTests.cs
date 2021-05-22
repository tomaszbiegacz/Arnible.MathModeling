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
    private readonly GoldenSecantSmoothlyConstrainedMinimum _optimizer;
    
    public GoldenSectionSmoothlyConstrainedMinimumTests(ITestOutputHelper output) : base(output)
    {
      _optimizer = new GoldenSecantSmoothlyConstrainedMinimum(Logger);
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

      NumberFunctionPointWithDerivative actual = _optimizer.MoveNext(f, in a, b.X);
      actual.X.AssertIsEqualTo(1);
    }

    [Fact]
    public void Unimodal_SquareReversed_Maximum()
    {
      var f = new SquareReversedTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(3);
      
      try
      {
        _optimizer.MoveNext(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(AssertException)
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
        _optimizer.MoveNext(f, in a, b.X);
        throw new Exception("I should never get here");
      }
      catch(AssertException)
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
      
      ushort iterationCount = _optimizer.FindOptimal(f, ref a, b.X);
      a.X.AssertIsEqualTo(-0.5 * Math.PI);
      iterationCount.AssertIsEqualTo(18);
    }
    
    [Fact]
    public void Multimodal_Sin()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1.1 * Math.PI);
      var b = f.ValueWithDerivative(2 * Math.PI);
      
      a.First.AssertIsLessThan(0);
      b.First.AssertIsGreaterThan(0);
      
      ushort iterationCount = _optimizer.FindOptimal(f, ref a, b.X);
      Assert.Equal(-0.5 * Math.PI, (double)a.X, 8);
      iterationCount.AssertIsEqualTo(15);
    }
    
    /*
     * Rosenbrock
     */

    [Fact]
    public void Multimodal_Rosenbrock_LocalMinimum()
    {
      var f = new RosenbrockTestFunction();
      FunctionValueAnalysisForDirection fa = new(
        f, 
        stackalloc Number[] { 0, 0 }, 
        OptimizationHelper.DirectionDerivativeRatiosD2.Span);
      
      var a = fa.ValueWithDerivative(0);
      var opt = fa.ValueWithDerivative(2 / Math.Sqrt(2));
      var b = fa.ValueWithDerivative(2);
      
      opt.Y.AssertIsEqualTo(0);
      opt.First.AssertIsEqualTo(0);
      
      a.Y.AssertIsGreaterThan(0);
      a.First.AssertIsLessThan(0);
      
      b.Y.AssertIsGreaterThan(0);
      b.First.AssertIsGreaterThan(0);
      
      ushort iterationCount = _optimizer.FindOptimal(in fa, ref a, b.X);
      a.X.AssertIsEqualTo(opt.X);
      iterationCount.AssertIsEqualTo(18);
    }
  }
}