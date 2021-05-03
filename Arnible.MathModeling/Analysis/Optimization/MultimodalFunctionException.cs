using System;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class MultimodalFunctionException : Exception
  {
    public MultimodalFunctionException()
      : base("Function is multimodal")
    {
      // intentionally empty
    }
  }
}