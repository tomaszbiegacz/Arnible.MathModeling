using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test.Functions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public class NumberFunctionOptimizationSearchRangeTests
  {
    private readonly SquareTestFunction _function = new SquareTestFunction();
    
    [Fact]
    public void ReadConstructorOrder()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3);
      NumberFunctionOptimizationSearchRange range = new(in a, in b);
      
      range.Start.X.AssertIsEqualTo(a.X);
      range.End.X.AssertIsEqualTo(b.X);
      range.Width.AssertIsEqualTo(3);
      range.IsEmptyRange.AssertIsFalse();
      
      range.BorderSmaller.X.AssertIsEqualTo(a.X);
      range.BorderGreater.X.AssertIsEqualTo(b.X);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(b.X);
    }
    
    [Fact]
    public void ReadConstructorInverseOrder()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(1.5);
      NumberFunctionOptimizationSearchRange range = new(in b, in a);
      
      range.Start.X.AssertIsEqualTo(a.X);
      range.End.X.AssertIsEqualTo(b.X);
      range.Width.AssertIsEqualTo(1.5);
      range.IsEmptyRange.AssertIsFalse();
      
      range.BorderSmaller.X.AssertIsEqualTo(b.X);
      range.BorderGreater.X.AssertIsEqualTo(a.X);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(b.X);
    }
    
    [Fact]
    public void ReadConstructorOptimum()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionOptimizationSearchRange range = new(in a, in a);
      
      range.Start.X.AssertIsEqualTo(a.X);
      range.End.X.AssertIsEqualTo(a.X);
      range.Width.AssertIsEqualTo(0);
      range.IsEmptyRange.AssertIsTrue();
      
      range.BorderSmaller.X.AssertIsEqualTo(a.X);
      range.BorderGreater.X.AssertIsEqualTo(a.X);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(a.X);
    }
    
    [Fact]
    public void AssignBorderSmaller()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3);
      NumberFunctionOptimizationSearchRange range = new(in a, in b);
      
      range.BorderSmaller.X.AssertIsEqualTo(a.X);
      range.BorderGreater.X.AssertIsEqualTo(b.X);
      
      range.BorderSmaller = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(1.5);
      
      range.BorderSmaller.X.AssertIsEqualTo(1.5);
      range.BorderGreater.X.AssertIsEqualTo(b.X);
    }
    
    [Fact]
    public void AssignBorderGreater()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3);
      NumberFunctionOptimizationSearchRange range = new(in a, in b);
      
      range.BorderSmaller.X.AssertIsEqualTo(a.X);
      range.BorderGreater.X.AssertIsEqualTo(b.X);
      
      range.BorderGreater = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3.5);
      
      range.BorderSmaller.X.AssertIsEqualTo(a.X);
      range.BorderGreater.X.AssertIsEqualTo(3.5);
    }
    
    [Fact]
    public void AssignBorderLowestDerivative()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3);
      NumberFunctionOptimizationSearchRange range = new(in a, in b);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(b.X);
      
      range.BorderLowestDerivative = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(1.5);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(1.5);
      range.BorderHighestDerivative.X.AssertIsEqualTo(b.X);
    }
    
    [Fact]
    public void AssignBorderGreatestDerivative()
    {
      NumberFunctionPointWithDerivative a = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(0);
      NumberFunctionPointWithDerivative b = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3);
      NumberFunctionOptimizationSearchRange range = new(in a, in b);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(b.X);
      
      range.BorderHighestDerivative = _function.FunctionValueAnalysisFor1D().ValueWithDerivative(3.5);
      
      range.BorderLowestDerivative.X.AssertIsEqualTo(a.X);
      range.BorderHighestDerivative.X.AssertIsEqualTo(3.5);
    }
  }
}