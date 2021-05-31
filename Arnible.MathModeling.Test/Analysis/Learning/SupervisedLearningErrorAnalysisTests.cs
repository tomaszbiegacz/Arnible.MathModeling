using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Learning;
using Arnible.MathModeling.Analysis.Learning.Error;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test.Learning
{
  public class SupervisedLearningErrorAnalysisTests
  {
    readonly static Number[] _inputs = new Number[] { 8, 9 };
    
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
    private readonly INumberRangeDomain _parametersDomain = new NumberDomain();
    private readonly IDifferentiableFunction _function = new SimpleFunction();
    
    [Fact]
    public void ErrorValue()
    {
      SupervisedLearningCase learningCase1 = new(Expected: 2, Input: _inputs);
      SupervisedLearningCase learningCase2 = new(Expected: 3, Input: _inputs);
      SupervisedLearningCase learningCase3 = new(Expected: 5, Input: _inputs);
      
      SupervisedLearningErrorAnalysis errorAnalysis = new(
        parametersDomain : _parametersDomain,
        errorMeasure: _measure,
        learningCases: new[] { learningCase1, learningCase2, learningCase3 },
        function: _function);
      
      object.ReferenceEquals(_parametersDomain, errorAnalysis.ParametersDomain).AssertIsTrue();
      object.ReferenceEquals(_measure, errorAnalysis.ErrorMeasure).AssertIsTrue();
      object.ReferenceEquals(_function, errorAnalysis.Function).AssertIsTrue();
      
      errorAnalysis.InputsCount.AssertIsEqualTo(2);
      errorAnalysis.LearningCases.Length.AssertIsEqualTo(3);
      
      errorAnalysis
        .GetValue(new Number[] { 1, 2 })
        .AssertIsEqualTo(9 + 4);
    }
    
    [Fact]
    public void ErrorDerivativeByParametersWithValue()
    {
      SupervisedLearningCase learningCase1 = new(Expected: 2, Input: _inputs);
      SupervisedLearningCase learningCase2 = new(Expected: 3, Input: _inputs);
      SupervisedLearningCase learningCase3 = new(Expected: 5, Input: _inputs);
      
      SupervisedLearningErrorAnalysis errorAnalysis = new(
        parametersDomain : _parametersDomain,
        errorMeasure: _measure,
        learningCases: new[] { learningCase1, learningCase2, learningCase3 },
        function: _function);
      
      ValueWithDerivative1 result = errorAnalysis
        .ErrorDerivativeByParametersWithValue(
          learningCase1,
          new Number[] { 1, 2 },
          new Number[] { 5, 6 }
        );
        
      result.Value.AssertIsEqualTo(9);
      result.First.AssertIsEqualTo(102);
    }
    
    [Fact]
    public void ValueWithDerivativeByArgumentsChangeDirection()
    {
      SupervisedLearningCase learningCase1 = new(Expected: 2, Input: _inputs);
      SupervisedLearningCase learningCase2 = new(Expected: 5, Input: _inputs);
      
      SupervisedLearningErrorAnalysis errorAnalysis = new(
        parametersDomain : _parametersDomain,
        errorMeasure: _measure,
        learningCases: new[] { learningCase1, learningCase2 },
        function: _function);
      
      ValueWithDerivative1 result = errorAnalysis
        .GetValueWithDerivativeByArgumentsChangeDirection(
          new Number[] { 1, 2 },
          new Number[] { 5, 6 }
        );
        
      result.Value.AssertIsEqualTo(9);
      result.First.AssertIsEqualTo(102);
    }
  }
}