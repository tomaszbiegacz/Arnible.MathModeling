using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public class RastriginTests
  {
    private readonly IFunctionValueAnalysis _function = new RastriginTestFunction();
    
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Optimum(ushort dimensionsCount)
    {
      Span<Number> solutionParameters = stackalloc Number[dimensionsCount];
      solutionParameters.Fill(0);
      
      Span<Number> solutionBuffer = stackalloc Number[dimensionsCount];
      FunctionMinimumImprovement solution = new(
        _function, 
        solutionParameters, 
        in solutionBuffer);
      
      solution.Value.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
  }
}