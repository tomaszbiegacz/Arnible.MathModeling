using System;
using Arnible.Assertions;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public class UnimodalSecantMinimumTests : TestsWithLogger
  {
    private readonly UnimodalSecantMinimum _method;
    
    public UnimodalSecantMinimumTests(ITestOutputHelper output) : base(output)
    {
      _method = new UnimodalSecantMinimum(Logger);
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
      
      _method.MoveNext(ref point);
      point.BorderSmaller.X.AssertIsEqualTo(1);
      point.BorderSmaller.Y.AssertIsEqualTo(3);
      
      point.IsOptimal.AssertIsTrue();
      try
      {
        _method.MoveNext(ref point);
        throw new Exception("I should never get here");
      }
      catch (AssertException)
      {
        // all is fine.
      }
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

      point.IsOptimal.AssertIsFalse();
      try
      {
        _method.MoveNext(ref point);  
        throw new Exception("I should never get here");
      }
      catch (MultimodalFunctionException)
      {
        // all is fine
      }
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
      i.AssertIsEqualTo(6);
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

      point.IsOptimal.AssertIsFalse();
      try
      {
        _method.MoveNext(ref point);  
        throw new Exception("I should never get here");
      }
      catch (MultimodalFunctionException)
      {
        // all is fine
      }
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

      point.IsOptimal.AssertIsFalse();
      try
      {
        _method.MoveNext(ref point);  
        throw new Exception("I should never get here");
      }
      catch (MultimodalFunctionException)
      {
        // all is fine
      }
    }
    
    /*
     * Multimodal sin
     */
    
    [Fact]
    public void Multimodal_Sin()
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

      point.IsOptimal.AssertIsFalse();
      try
      {
        _method.MoveNext(ref point);  
        throw new Exception("I should never get here");
      }
      catch (MultimodalFunctionException)
      {
        // all is fine
      }
    }
  }
}