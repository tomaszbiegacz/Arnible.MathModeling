using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public class BealeTests
  {
    private readonly IFunctionValueAnalysis _function = new BealeTestFunction();
    
    [Fact]
    public void Optimum()
    {
      Span<Number> solutionParameters = stackalloc Number[2];
      solutionParameters[0] = 3;
      solutionParameters[1] = 0.5;
      
      Span<Number> solutionBuffer = stackalloc Number[2];
      FunctionMinimumImprovement solution = new(
        _function, 
        solutionParameters, 
        in solutionBuffer);
      
      solution.Value.AssertIsEqualTo(0);
      solution.Function.IsOptimum(solution.Parameters).AssertIsTrue();
    }
  }
}