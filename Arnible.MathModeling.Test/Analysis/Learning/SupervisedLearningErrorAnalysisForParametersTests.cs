using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Learning;
using Arnible.MathModeling.Analysis.Learning.Error;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test.Learning
{
  public class SupervisedLearningErrorAnalysisForParametersTests
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
    
    private SupervisedLearningErrorAnalysis GetErrorAnalysis()
    {
      SupervisedLearningCase learningCase1 = new(Expected: 2, Input: _inputs);
      SupervisedLearningCase learningCase2 = new(Expected: 5, Input: _inputs);
  
      return new(
        parametersDomain : _parametersDomain,
        errorMeasure: _measure,
        learningCases: new[] { learningCase1, learningCase2 },
        function: _function);
    }
    
    [Fact]
    public void ErrorValue()
    {
      SupervisedLearningErrorAnalysisForParameters analysis = GetErrorAnalysis()
        .ForParameters(new Number[] { 1, 2 });
      
      analysis.ErrorValue.AssertIsEqualTo(9);
    }
    
    [Fact]
    public void ErrorGradientByParameters()
    {
      SupervisedLearningErrorAnalysisForParameters analysis = GetErrorAnalysis()
        .ForParameters(new Number[] { 1, 2 });
      
      Span<Number> gradient = stackalloc Number[2];
      analysis.ErrorGradientByParameters(gradient);
      
      gradient.AssertSequenceEqualsTo(new Number[] { 6, 12 });
    }
    
    [Fact]
    public void ErrorDerivativeByParameters()
    {
      SupervisedLearningErrorAnalysisForParameters analysis = GetErrorAnalysis()
        .ForParameters(new Number[] { 1, 2 });
      
      analysis.ErrorDerivativeByParameters(new Number[] { 5, 6 }).AssertIsEqualTo(102);
    }
  }
}