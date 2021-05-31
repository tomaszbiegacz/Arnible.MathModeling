using System;
using Arnible.Assertions;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Analysis.Learning
{
  public readonly ref struct SupervisedLearningErrorAnalysisForParameters
  {
    public SupervisedLearningErrorAnalysis Parent { get; }
    
    public ReadOnlySpan<Number> Parameters { get; }
    
    public Number ErrorValue { get; }
    
    public SupervisedLearningErrorAnalysisForParameters(
      SupervisedLearningErrorAnalysis parent,
      in ReadOnlySpan<Number> parameters)
    {
      Parent = parent;
      
      Parameters = parameters;
      ErrorValue = Parent.ErrorValue(in parameters);
    }

    /// <summary>
    /// Error gradient by parameters
    /// </summary>
    public void ErrorGradientByParameters(in Span<Number> output)
    {
      Span<Number> itemGradient = stackalloc Number[output.Length];
      
      output.Clear();
      foreach(ref readonly var learningCase in Parent.LearningCases.Span)
      {
        Parent.Function.GetParametersGradient(
          parameters: Parameters,
          inputs: learningCase.Input.Span,
          in itemGradient);
        
        Derivative1Value errorDerivative = learningCase.ErrorMeasureDerivativeByOutput(
          Parent.Function,
          Parent.ErrorMeasure,
          Parameters);
        itemGradient.MultiplySelf(errorDerivative.First);
        
        output.AddSelf(itemGradient);
      }
    }
    
    /// <summary>
    /// Error derivative by parameters changing in given direction
    /// </summary>
    public Number ErrorDerivativeByParameters(in ReadOnlySpan<Number> pRatio)
    {
      pRatio.Length.AssertIsEqualTo(Parameters.Length);
      
      Span<Number> xDerivative = stackalloc Number[Parent.InputsCount];
      xDerivative.Clear();

      Number result = 0;
      foreach(ref readonly var learningCase in Parent.LearningCases.Span)
      {
        Number valueDerivative = Parent.Function.DerivativeByParameters(
          parameters: Parameters,
          pRatio: in pRatio,
          inputs: learningCase.Input.Span,
          inputsDerivative: xDerivative
        );
        
        Derivative1Value errorDerivativeByOutput = learningCase.ErrorMeasureDerivativeByOutput(
          Parent.Function,
          Parent.ErrorMeasure,
          Parameters);
        
        result += valueDerivative * errorDerivativeByOutput.First;
      }
      return result;
    }
  }
}