﻿namespace Arnible.MathModeling.Analysis
{
  public static class OperationExtension
  {
    public static double Value(
      this IFinitaryOperation<double> operation,
      params double[] x)
    {
      return operation.Value(x);
    }
  }
}
