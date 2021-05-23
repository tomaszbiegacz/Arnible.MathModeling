using System;
using Arnible.Assertions;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Analysis.Learning;
using Arnible.MathModeling.Analysis.Learning.Error;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test.Learning
{
  public class SupervisedLearningCaseTests
  {
    readonly static Number[] _inputs = new Number[] { 3, 4 };
    
    record SimpleFunction : IDifferentiableFunction
    {
      public Number Value(
        in ReadOnlySpan<Number> parameters, 
        in ReadOnlySpan<Number> inputs)
      {
        inputs.AssertSequenceEqualsTo(_inputs);
        return parameters[0] + 2 * parameters[1];
      }

      public void GetParametersGradient(
        in ReadOnlySpan<Number> parameters, 
        in ReadOnlySpan<Number> inputs, 
        in Span<Number> output)
      {
        inputs.AssertSequenceEqualsTo(_inputs);
        output[0] = 1;
        output[1] = 2;
      }

      public Number DerivativeByParameters(
        in ReadOnlySpan<Number> parameters, 
        in ReadOnlySpan<Number> pRatio, 
        in ReadOnlySpan<Number> inputs,
        in ReadOnlySpan<Number> inputsDerivative)
      {
        inputs.AssertSequenceEqualsTo(_inputs);
        return pRatio[0] + 2*pRatio[1];
      }

      public ValueWithDerivative1 DerivativeByParametersWithValue(
        in ReadOnlySpan<Number> parameters, 
        in ReadOnlySpan<Number> pRatio,
        in ReadOnlySpan<Number> inputs, 
        in ReadOnlySpan<Number> inputsDerivative)
      {
        return new()
        {
          Value = Value(parameters, inputs),
          First = DerivativeByParameters(parameters, pRatio, inputs, inputsDerivative)
        };
      }
    }
    
    private readonly SquareErrorMeasure _measure = new();
    
    [Fact]
    public void ErrorValue()
    {
      SupervisedLearningCase learningCase = new(Expected: 2, Input: new Number[] { 3, 4 });
      learningCase
        .ErrorValue(new SimpleFunction(), _measure, new Number[] { 1, 2 })
        .AssertIsEqualTo(9);
    }
    
    [Fact]
    public void ErrorMeasureDerivativeByOutput()
    {
      SupervisedLearningCase learningCase = new(Expected: 2, Input: new Number[] { 3, 4 });
      learningCase
        .ErrorMeasureDerivativeByOutput(new SimpleFunction(), _measure, new Number[] { 1, 2 })
        .First
        .AssertIsEqualTo(6);
    }
  }
}