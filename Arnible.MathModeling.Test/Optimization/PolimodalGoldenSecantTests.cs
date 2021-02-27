using System;
using Arnible.MathModeling.xunit;
using Xunit;
using Xunit.Abstractions;
using static Arnible.MathModeling.xunit.AssertNumber;

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
      AreExactlyEqual(2, method.X);
      AreExactlyEqual(4, method.Y);
      
      IsTrue(method.MoveNext());
      AreEqual(1, method.X);
      AreEqual(3, method.Y);
      
      IsTrue(method.IsOptimal);
      IsFalse(method.MoveNext());
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1.5);
      var b = f.ValueWithDerivative(2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(1.5, method.X);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(1.5, method.X);
      AreEqual(21, i);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(1, method.X);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(1, method.X);
      AreEqual(22, i);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(0.5);
      var b = f.ValueWithDerivative(-2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(0.5, method.X);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(0.5, method.X);
      AreEqual(24, i);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative_Optimum()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(1);
      var b = f.ValueWithDerivative(-2);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(1, method.X);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(1, method.X);
      AreEqual(23, i);
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
      AreExactlyEqual(-1, method.X);
      AreExactlyEqual(-1, method.Y);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(-1, method.X);
      AreEqual(23, i);
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
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(2, method.Y);
      AreEqual(6, i);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerDecreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0.3 * Math.PI);
      var b = f.ValueWithDerivative(Math.PI);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(b.X, method.X);
      AreExactlyEqual(b.Y, method.Y);
      IsGreaterThan(b.Y, a.Y);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(3, method.Y);
      AreEqual(22, i);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerIncreasing()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(0);
      var b = f.ValueWithDerivative(0.7 * Math.PI);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);
      IsGreaterThan(a.Y, b.Y);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(3, method.Y);
      AreEqual(23, i);
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
      
      IsLowerThan(b.Y, a.Y);
      IsLowerThan(0, a.First);
      IsGreaterThan(0, b.First);
      
      var method = new PolimodalGoldenSecant(f: f, a: a, b: b, Logger);
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);

      uint i = OptimizationHelper.FindOptimal(method);
      
      AreEqual(2, method.Y);
      AreEqual(6, i);
    }
  }
}