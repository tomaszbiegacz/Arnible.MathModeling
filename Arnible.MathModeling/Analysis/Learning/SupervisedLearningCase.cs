using System;

namespace Arnible.MathModeling.Analysis.Learning
{
  public record SupervisedLearningCase(in Number Expected, in ReadOnlyMemory<Number> Input)
  {
    public ReadOnlyMemory<Number> Input { get; } = Input;

    public Number Expected { get; }  = Expected;
    
    public Number ErrorValue(
      IDifferentiableFunction function,
      IErrorMeasureSupervisedLearning<Number> errorMeasure,
      in ReadOnlySpan<Number> parameters)
    {
      return errorMeasure.ErrorValue(
        expected: Expected,
        actual: function.Value(parameters: in parameters, inputs: Input.Span));
    }
    
    public Derivative1Value ErrorMeasureDerivativeByOutput(
        IDifferentiableFunction function,
        IErrorMeasureSupervisedLearning<Number> errorMeasure,
        in ReadOnlySpan<Number> parameters)
    {
      return errorMeasure.ErrorDerivativeByActual(
        expected: Expected, 
        actual: function.Value(parameters: in parameters, inputs: Input.Span));
    }
  }
}
