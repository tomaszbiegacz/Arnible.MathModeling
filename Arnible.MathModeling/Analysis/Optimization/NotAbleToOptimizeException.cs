using System;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class NotAbleToOptimizeException : Exception
  {
    public NotAbleToOptimizeException()
      : base("Not able to optimize")
    {
      // intentionally empty
    }
  }
}