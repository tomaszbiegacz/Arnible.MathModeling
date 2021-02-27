using System;
using Arnible.MathModeling.Optimization.Test;
using Arnible.MathModeling.xunit;
using Xunit;
using Xunit.Abstractions;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Optimization.SingleStep.Test
{
  public class GoldenSectionSmoothlyConstrainedTests : TestsWithLogger
  {
    public GoldenSectionSmoothlyConstrainedTests(ITestOutputHelper output) : base(output)
    {
    }

    private ISingleStepOptimization CreateOptimizer()
    {
      return new GoldenSectionSmoothlyConstrained(Logger);
    }

    /// <summary>
    /// Use secant to find optimum in one step
    /// </summary>
    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var optimizer = CreateOptimizer();
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(3);

      Number actual = optimizer.Optimize(f, in a, b.X);
      AreEqual(1, actual);
    }
    
    [Fact]
    public void Unimodal_SquareReversed_Maximum()
    {
      var optimizer = CreateOptimizer();
      var f = new SquareReversedTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(3);
      
      Assert.Throws<NotAbleToOptimizeException>(() => optimizer.Optimize(f, a, b.X).ToString());
    }
    
    [Fact]
    public void Unimodal_Square_WrongDirection_PositiveDerivative()
    {
      var optimizer = CreateOptimizer();
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);

      Assert.Throws<NotAbleToOptimizeException>(() => optimizer.Optimize(f, a, b.X).ToString());
    }
    
    [Fact]
    public void Unimodal_Square_WrongDirection_NegativeDerivative()
    {
      var optimizer = CreateOptimizer();
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(0.5);
      var b = f.ValueWithDerivative(-2);

      Assert.Throws<NotAbleToOptimizeException>(() => optimizer.Optimize(f, a, b.X).ToString());
    }
    
    [Fact]
    public void Unimodal_Square_AtOptimum()
    {
      var optimizer = CreateOptimizer();
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(2);
      
      Assert.Throws<NotAbleToOptimizeException>(() => optimizer.Optimize(f, a, b.X).ToString());
    }
    
    [Fact]
    public void Unimodal_Sin_Optimum()
    {
      var optimizer = CreateOptimizer();
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(-1.3 * Math.PI);
      var b = f.ValueWithDerivative(0.4 * Math.PI);
      
      double actual = (double)optimizer.Optimize(f, in a, b.X);
      Assert.Equal(-0.5 * Math.PI, actual, precision: 1);
    }
    
    [Fact]
    public void Polimodal_Sin()
    {
      var optimizer = CreateOptimizer();
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(-1.1 * Math.PI);
      var b = f.ValueWithDerivative(2 * Math.PI);
      
      IsLowerThan(0, a.First);
      IsGreaterThan(0, b.First);
      
      double actual = (double)optimizer.Optimize(f, in a, b.X);
      Assert.Equal(-0.5 * Math.PI, actual, precision: 1);
    }
  }
}