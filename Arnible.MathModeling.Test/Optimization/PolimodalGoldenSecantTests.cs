using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Optimization.Test
{
  public class PolimodalGoldenSecantTests : TestsWithLogger
  {
    public PolimodalGoldenSecantTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(2);
      method.Y.AssertIsEqualTo(4);
      
      ConditionExtensions.AssertIsTrue(method.MoveNext());
      method.X.AssertIsEqualTo(1);
      method.Y.AssertIsEqualTo(3);
      
      ConditionExtensions.AssertIsTrue(method.IsOptimal);
      ConditionExtensions.AssertIsFalse(method.MoveNext());
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(6);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerDecreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0.3 * Math.PI);
      var b = f.ValueWithDerivative(Math.PI);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(b.X);
      method.Y.AssertIsEqualTo(b.Y);
      IsLessThanExtensions.AssertIsLessThan(b.Y, a.Y);

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
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);
      IsLessThanExtensions.AssertIsLessThan(a.Y, b.Y);

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
      
      IsGreaterThanExtensions.AssertIsGreaterThan(b.Y, a.Y);
      IsGreaterThanExtensions.AssertIsGreaterThan(0, a.First);
      IsLessThanExtensions.AssertIsLessThan(0, b.First);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      method.X.AssertIsEqualTo(a.X);
      method.Y.AssertIsEqualTo(a.Y);

      ushort i = OptimizationHelper.FindOptimal(method);
      
      method.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(6);
    }
  }
}