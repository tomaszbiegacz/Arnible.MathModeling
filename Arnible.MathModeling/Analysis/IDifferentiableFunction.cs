using System;

namespace Arnible.MathModeling.Analysis
{
  public interface IDifferentiableFunction
  {
    Number Value(
      in ReadOnlySpan<Number> parameters,
      in ReadOnlySpan<Number> inputs);
    
    void GetParametersGradient(
      in ReadOnlySpan<Number> parameters, 
      in ReadOnlySpan<Number> inputs,
      in Span<Number> output);
    
    Number DerivativeByParameters(
      in ReadOnlySpan<Number> parameters,
      in ReadOnlySpan<Number> pRatio,
      in ReadOnlySpan<Number> inputs,
      in ReadOnlySpan<Number> inputsDerivative);
    
    /// <summary>
    /// Value with derivative when moving parameters in give direction 
    /// </summary>
    ValueWithDerivative1 DerivativeByParametersWithValue(
      in ReadOnlySpan<Number> parameters,
      in ReadOnlySpan<Number> pRatio,
      in ReadOnlySpan<Number> inputs,
      in ReadOnlySpan<Number> inputsDerivative);
  }
}