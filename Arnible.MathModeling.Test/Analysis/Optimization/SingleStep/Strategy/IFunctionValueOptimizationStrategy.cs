namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public interface IFunctionValueOptimizationStrategy
  {
    void FindImprovedArguments(in FunctionMinimumImprovement solution);
  }
}