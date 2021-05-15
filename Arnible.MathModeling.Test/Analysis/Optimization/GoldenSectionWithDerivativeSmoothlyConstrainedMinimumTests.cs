using System;
using Arnible.Assertions;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public class GoldenSectionWithDerivativeSmoothlyConstrainedMinimumTests : TestsWithLogger
  {
    private readonly GoldenSectionWithDerivativeSmoothlyConstrainedMinimum _method;
    
    public GoldenSectionWithDerivativeSmoothlyConstrainedMinimumTests(ITestOutputHelper output) : base(output)
    {
      _method = new(Logger);
    }

    [Fact]
    public void Unimodal_Square_Optimum()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1);
      Number b = 2;

      ushort i = _method.FindOptimal(
        in f,
        a,
        b,
        out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1.5);
      Number b = 2;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(1.5);
      i.AssertIsEqualTo(21);
    }
    
    [Fact]
    public void Unimodal_Square_PositiveDerivative_Optimum()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1);
      Number b = 2;

      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(0.5);
      Number b = -2;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(0.5);
      i.AssertIsEqualTo(24);
    }
    
    [Fact]
    public void Unimodal_Square_NegativeDerivative_Optimum()
    {
      var f = new SquareTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1);
      Number b = -2;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(1);
      i.AssertIsEqualTo(24);
    }
    
    /*
     * Unimodal square reversed
     */
    
    [Fact]
    public void Unimodal_SquareReversed()
    {
      var f = new SquareReversedTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1);
      Number b = 2;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.X.AssertIsEqualTo(-1);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Unimodal sin
     */
    
    [Fact]
    public void Unimodal_Sin_Optimum()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(-1.3 * Math.PI);
      Number b = 0.4 * Math.PI;

      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(21);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerDecreasing()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(0.3 * Math.PI);
      Number b = Math.PI;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(22);
    }
    
    [Fact]
    public void Unimodal_Sin_CornerIncreasing()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(0);
      Number b = 0.7 * Math.PI;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.Y.AssertIsEqualTo(3);
      i.AssertIsEqualTo(23);
    }
    
    /*
     * Multimodal sin
     */
    
    [Fact]
    public void Multimodal_Sin()
    {
      var f = new SinTestFunction().FunctionValueAnalysisFor1D();
      var a = f.ValueWithDerivative(1.4 * Math.PI);
      Number b = 4.4 * Math.PI;
      
      ushort i = _method.FindOptimal(in f, a, b, out NumberFunctionPointWithDerivative solution);
      
      solution.Y.AssertIsEqualTo(2);
      i.AssertIsEqualTo(22);
    }
  }
}