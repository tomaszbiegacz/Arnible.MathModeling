using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization;
using Arnible.MathModeling.Test;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Optimization.Test
{
  public class GoldenSectionConstrainedTests : TestsWithLogger
  {
    public GoldenSectionConstrainedTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(2);
      method.Y.AssertIsEqualTo(4);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(26);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(1.5);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(1.5);
      i.AssertIsEqualTo(21);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(1);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(0.5);
      var b = f.ValueWithDerivative(-2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(0.5);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(0.5);
      i.AssertIsEqualTo(24);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(-2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(1);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Unimodal square reversed
     */
    
    [Fact]
    public void Unimodal_SquareReversed()
    {
      var f = new SquareReversedTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(-1);
      method.Y.AssertIsEqualTo(-1);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.X.AssertIsEqualTo(-1);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Unimodal sin
     */
    
    [Fact]
    public void Unimodal_Sin_Optimum()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(-1.3 * Math.PI);
      var b = f.ValueWithDerivative(0.4 * Math.PI);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerDecreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0.3 * Math.PI);
      var b = f.ValueWithDerivative(Math.PI);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(b.X);
      method.Y.AssertIsEqualTo(b.Y);
      a.Y.AssertIsGreaterThan(b.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerIncreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0);
      var b = f.ValueWithDerivative(0.7 * Math.PI);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);
      b.Y.AssertIsGreaterThan(a.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Polimodal sin
     */
    
    [Fact]
    public void Polimodal_Sin()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(1.4 * Math.PI);
      var b = f.ValueWithDerivative(4.4 * Math.PI);
      
      a.Y.AssertIsLessThan(b.Y);
      a.First.AssertIsLessThan(0);
      b.First.AssertIsGreaterThan(0);
      
      var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(22);
    }
  }
}