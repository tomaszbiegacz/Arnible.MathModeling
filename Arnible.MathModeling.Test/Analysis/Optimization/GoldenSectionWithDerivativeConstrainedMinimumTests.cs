using System;
using Arnible.Assertions;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public class GoldenSectionWithDerivativeConstrainedMinimumTests : TestsWithLogger
  {
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _method;
    
    public GoldenSectionWithDerivativeConstrainedMinimumTests(ITestOutputHelper output) : base(output)
    {
      _method = new GoldenSectionWithDerivativeConstrainedMinimum(Logger);
    }

    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(2);
      point.BorderSmaller.Y.AssertIsEqualTo(4);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(1.5);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(1.5);
      i.AssertIsEqualTo(21);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(2);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(1);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(0.5);
      var b = f.ValueWithDerivative(-2);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(0.5);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(0.5);
      i.AssertIsEqualTo(24);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(-2);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(1);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(24);
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
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(-1);
      point.BorderSmaller.Y.AssertIsEqualTo(-1);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.X.AssertIsEqualTo(-1);
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
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(a.X);
      point.BorderSmaller.Y.AssertIsEqualTo(a.Y);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(21);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerDecreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0.3 * Math.PI);
      var b = f.ValueWithDerivative(Math.PI);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(b.X);
      point.BorderSmaller.Y.AssertIsEqualTo(b.Y);
      a.Y.AssertIsGreaterThan(b.Y);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerIncreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0);
      var b = f.ValueWithDerivative(0.7 * Math.PI);
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(a.X);
      point.BorderSmaller.Y.AssertIsEqualTo(a.Y);
      b.Y.AssertIsGreaterThan(a.Y);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Multimodal sin
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
      
      var point = new NumberFunctionOptimizationSearchRange(f: f, a: a, b: b);
      point.BorderSmaller.X.AssertIsEqualTo(a.X);
      point.BorderSmaller.Y.AssertIsEqualTo(a.Y);

      ushort i = _method.FindOptimal(ref point);
      
      point.BorderSmaller.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(22);
    }
  }
}