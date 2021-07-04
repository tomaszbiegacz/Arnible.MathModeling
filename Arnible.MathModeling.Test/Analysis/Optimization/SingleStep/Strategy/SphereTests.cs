using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Optimization.Test.Functions;
using Arnible.MathModeling.Test;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  [Collection(nameof(SphereTests))]
  public class SphereTests : TestsWithWriterFactory
  {
    private readonly IFunctionValueAnalysis _function;

    public SphereTests(ITestOutputHelper output)
      : base(output)
    {
      _function = new SphereTestFunction();
    }

    [Theory]
    [InlineData(2, 2, false)]
    [InlineData(3, 2, false)]
    [InlineData(4, 2, false)]
    [InlineData(5, 2, false)]
    [InlineData(6, 2, false)]
    [InlineData(7, 2, false)]
    [InlineData(8, 2, false)]
    [InlineData(9, 2, false)]
    [InlineData(10, 2, false)]
    public void Xp_Yp_ExtendedUniformSearchDirection(
      ushort dimensionsCount,
      ushort iterationsCount,
      bool saveLogsToFile)
    {
      if(saveLogsToFile)
      {
        BackupLogsToFile();
      }
      Span<Number> parameters = stackalloc Number[dimensionsCount];
      parameters.Fill(4);

      Span<Number> solutionBuffer = stackalloc Number[dimensionsCount];
      FunctionMinimumImprovement solution = new(
        _function, 
        parameters, 
        in solutionBuffer);
      
      GoldenSecantStrategy strategy = new(-6, 6, Logger)
      {
        UniformSearchDirection = true
      };
      ushort iterations = strategy.FindOptimal(Logger, ref solution);
      
      for(ushort i=0; i<dimensionsCount; ++i)
      {
        Assert.Equal(0, solution.Parameters[i]);  
      }
      iterations.AssertIsEqualTo(iterationsCount);
    }
  }
}