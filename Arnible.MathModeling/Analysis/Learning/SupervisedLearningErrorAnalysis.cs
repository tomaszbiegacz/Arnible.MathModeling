using System;
using Arnible.Linq;
using Arnible.MathModeling.Analysis.Optimization;

namespace Arnible.MathModeling.Analysis.Learning
{
  public record SupervisedLearningErrorAnalysis : IFunctionValueAnalysis
  {
    public SupervisedLearningErrorAnalysis(
      INumberRangeDomain parametersDomain,
      IErrorMeasureSupervisedLearning<Number> errorMeasure,
      in ReadOnlyMemory<SupervisedLearningCase> learningCases,
      IDifferentiableFunction function)
    {
      ParametersDomain = parametersDomain;
      ErrorMeasure = errorMeasure;
      LearningCases = learningCases;
      Function = function;
      InputsCount = (ushort)learningCases.Span.Distinct((in SupervisedLearningCase x) => x.Input.Length).Single(); 
    }
    
    /*
     * Properties
     */
    
    public INumberRangeDomain ParametersDomain { get; }
    public IErrorMeasureSupervisedLearning<Number> ErrorMeasure { get; }
    public ReadOnlyMemory<SupervisedLearningCase> LearningCases { get; }
    public IDifferentiableFunction Function { get; }
    public ushort InputsCount { get; }
    
    /*
     * Queries
     */

    public Number ErrorValue(in ReadOnlySpan<Number> parameters)
    {
      Number result = 0;
      foreach(ref readonly SupervisedLearningCase learningCase in LearningCases.Span)
      {
        result += learningCase.ErrorValue(Function, ErrorMeasure, in parameters);
      }
      return result;
    }
    
    public SupervisedLearningErrorAnalysisForParameters ForParameters(in ReadOnlySpan<Number> parameters)
    {
      return new SupervisedLearningErrorAnalysisForParameters(this, in parameters);
    }
    
    public ValueWithDerivative1 ErrorDerivativeByParametersWithValue(
      SupervisedLearningCase learningCase,
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> pRatio)
    {
      Span<Number> xDerivative = stackalloc Number[InputsCount];
      xDerivative.Clear();
      
      ValueWithDerivative1 do_withValue = Function.DerivativeByParametersWithValue(
        parameters: in parameters,
        pRatio: in pRatio,
        inputs: learningCase.Input.Span,
        inputsDerivative: xDerivative
      );

      Number de_value = ErrorMeasure.ErrorValue(
        expected: learningCase.Expected,
        actual: do_withValue.Value);
      
      Number de_do = ErrorMeasure.ErrorDerivativeByActual(
        expected: learningCase.Expected,
        actual: do_withValue.Value
      ).First;

      return new ValueWithDerivative1
      {
        Value = de_value,
        First = de_do * do_withValue.First
      };
    }
    
    public ValueWithDerivative1 ValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      Number resultValue = 0;
      Number resultFirst = 0;
      foreach(ref readonly var learningCase in LearningCases.Span)
      {
        var singleResult = ErrorDerivativeByParametersWithValue(
          learningCase,
          parameters: in parameters,
          pRatio: in directionDerivativeRatios);
        
        resultValue += singleResult.Value;
        resultFirst += singleResult.First;
      }
      return new ValueWithDerivative1
      {
        Value = resultValue,
        First = resultFirst
      };
    }
  }
}