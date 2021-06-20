namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public interface IFunctionValueOptimizationStrategy
  {
    FunctionImprovementStatistics FindImprovedArguments(ref FunctionMinimumImprovement solution);
  }
}