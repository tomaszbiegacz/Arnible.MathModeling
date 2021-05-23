using System;

namespace Arnible.MathModeling.Analysis.Learning
{
  /// <summary>
  /// Error measure for supervised learning
  /// </summary>
  public interface IErrorMeasureSupervisedLearning<TOutput> where TOutput : struct, IEquatable<TOutput>
  {    
    /// <summary>
    /// Error value calculation
    /// </summary>
    Number ErrorValue(in TOutput expected, in TOutput actual);

    /// <summary>
    /// Derivative of an error value by actual
    /// </summary>
    Derivative1Value ErrorDerivativeByActual(in TOutput expected, in TOutput actual);
  }
}
