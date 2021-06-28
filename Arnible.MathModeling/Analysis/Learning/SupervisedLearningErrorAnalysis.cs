using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Analysis.Optimization;

namespace Arnible.MathModeling.Analysis.Learning
{
  /// <summary>
  /// Stateless error analysis 
  /// </summary>
  public record SupervisedLearningErrorAnalysis : IFunctionValueAnalysis
  {
    public SupervisedLearningErrorAnalysis(
      IDifferentiableFunction function,
      INumberRangeDomain parametersDomain,
      IErrorMeasureSupervisedLearning<Number> errorMeasure,
      in ReadOnlyMemory<SupervisedLearningCase> learningCases)
    {
      learningCases.Length.AssertIsGreaterThan(0);
      
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

    public Number GetValue(in ReadOnlySpan<Number> parameters)
    {
      Number result = 0;
      foreach(ref readonly SupervisedLearningCase learningCase in LearningCases.Span)
      {
        result += learningCase.ErrorValue(Function, ErrorMeasure, in parameters);
      }
      return result;
    }
    
    public ValueWithDerivative1 ErrorDerivativeByParametersWithValue(
      SupervisedLearningCase learningCase,
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> pRatio)
    {
      Span<Number> xDerivative = stackalloc Number[InputsCount];
      xDerivative.Clear();
      return ErrorDerivativeByParametersWithValue(learningCase, in parameters, in pRatio, xDerivative);
    }

    public ValueWithDerivative1 ErrorDerivativeByParametersWithValue(
      SupervisedLearningCase learningCase,
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> pRatio,
      in ReadOnlySpan<Number> xDerivative)
    {
      ValueWithDerivative1 do_withValue = Function.DerivativeByParametersWithValue(
        parameters: in parameters,
        pRatio: in pRatio,
        inputs: learningCase.Input.Span,
        inputsDerivative: in xDerivative
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
    
    public ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      Span<Number> xDerivative = stackalloc Number[InputsCount];
      xDerivative.Clear();
      
      Number resultValue = 0;
      Number resultFirst = 0;
      foreach(ref readonly var learningCase in LearningCases.Span)
      {
        var singleResult = ErrorDerivativeByParametersWithValue(
          learningCase,
          parameters: in parameters,
          pRatio: in directionDerivativeRatios,
          xDerivative: xDerivative);
        
        resultValue += singleResult.Value;
        resultFirst += singleResult.First;
      }
      return new ValueWithDerivative1
      {
        Value = resultValue,
        First = resultFirst
      };
    }

    public void GradientByArguments(
      in ReadOnlySpan<Number> parameters, 
      in Span<Number> output)
    {
      Span<Number> itemGradient = stackalloc Number[output.Length];
      
      output.Clear();
      foreach(ref readonly var learningCase in LearningCases.Span)
      {
        Function.GetParametersGradient(
          parameters: in parameters,
          inputs: learningCase.Input.Span,
          in itemGradient);
        
        Derivative1Value errorDerivative = learningCase.ErrorMeasureDerivativeByOutput(
          Function,
          ErrorMeasure,
          in parameters);
        itemGradient.MultiplySelfBy(errorDerivative.First);
        
        output.AddToSelf(in itemGradient);
      }
    }
  }
}