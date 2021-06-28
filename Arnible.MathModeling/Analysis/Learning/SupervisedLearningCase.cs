using System;

namespace Arnible.MathModeling.Analysis.Learning
{
  public record SupervisedLearningCase(in Number Expected, in ReadOnlyMemory<Number> Input)
  {
    private readonly Number _expected = Expected;
    private readonly ReadOnlyMemory<Number> _input = Input;

    public ref readonly ReadOnlyMemory<Number> Input => ref _input;

    public ref readonly Number Expected => ref _expected;
    
    public Number ErrorValue(
      IDifferentiableFunction function,
      IErrorMeasureSupervisedLearning<Number> errorMeasure,
      in ReadOnlySpan<Number> parameters)
    {
      return errorMeasure.ErrorValue(
        expected: in _expected,
        actual: function.Value(parameters: in parameters, inputs: Input.Span));
    }
    
    public Derivative1Value ErrorMeasureDerivativeByOutput(
        IDifferentiableFunction function,
        IErrorMeasureSupervisedLearning<Number> errorMeasure,
        in ReadOnlySpan<Number> parameters)
    {
      return errorMeasure.ErrorDerivativeByActual(
        expected: in _expected, 
        actual: function.Value(parameters: in parameters, inputs: Input.Span));
    }
  }
}
