using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test.Functions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public class FunctionMinimumImprovementTests
  {
    [Fact]
    public void Basic()
    {
      IFunctionValueAnalysis f = new SinCos2DTestFunction();
      FunctionMinimumImprovement v = new(
        function: f,
        sourceParameters: new Number[] { -1, -1 },
        solutionBuffer: new Number[2],
        conjugateDirection: default);
      
      v.HasConjugateDirections.AssertIsFalse();
      v.FinaliseCurrentDirectionSearch(new Number[] { 1, 2 });
      v.FinaliseCurrentDirectionSearch(new Number[] { 2, 4 });
      v.HasConjugateDirections.AssertIsFalse();
      
      try
      {
        v.GetConjugateDirection(new Number[2]);
        throw new Exception("Something is wrong");
      }
      catch(AssertException)
      {
        // all is OK
      }
    }
    
    [Fact]
    public void LastTwoDirections()
    {
      IFunctionValueAnalysis f = new SinCos2DTestFunction();
      
      FunctionMinimumImprovement v = new(
        function: f,
        sourceParameters: new Number[] { -1, -1 },
        solutionBuffer: new Number[2],
        conjugateDirection: new(
          new Span2D<Number>(new Number[4], columnsCount: 2),
          new SpanSingle<ushort>(new ushort[1]),
          new SpanSingle<bool>(new bool[1])));
      
      v.HasConjugateDirections.AssertIsFalse();
      v.FinaliseCurrentDirectionSearch(new Number[] { 1, 2 });
      v.FinaliseCurrentDirectionSearch(new Number[] { 2, 4 });
      v.HasConjugateDirections.AssertIsTrue();
      v.FinaliseCurrentDirectionSearch(new Number[] { 4, 8 });

      v.GetConjugateDirection(new Number[3]).AssertSequenceEqualsTo(new Number[] { -8, -14 });
    } 
  }
}