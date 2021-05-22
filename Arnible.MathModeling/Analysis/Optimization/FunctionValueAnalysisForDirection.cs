using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public readonly ref struct FunctionValueAnalysisForDirection
  {
      private readonly IFunctionValueAnalysis _functionToAnalyse;
      private readonly ReadOnlySpan<Number> _startPosition;
      private readonly ReadOnlySpan<Number> _directionDerivativeRatios;
      
      public FunctionValueAnalysisForDirection(
        IFunctionValueAnalysis functionToAnalyse,
        in ReadOnlySpan<Number> startPosition,
        in ReadOnlySpan<Number> directionDerivativeRatios)
      {
        startPosition.Length.AssertIsEqualTo(directionDerivativeRatios.Length);
        
        _functionToAnalyse = functionToAnalyse;
        _startPosition = startPosition;
        _directionDerivativeRatios = directionDerivativeRatios;
      }
      
      public void GetPosition(in Number x, in Span<Number> result)
      {
        result.Length.AssertIsEqualTo(_startPosition.Length);
        for (ushort i = 0; i < _startPosition.Length; ++i)
        {
          result[i] = _startPosition[i] + x * _directionDerivativeRatios[i];
        }
      }
    
      public NumberFunctionPointWithDerivative ValueWithDerivative(in Number x)
      {
        Span<Number> parameters = stackalloc Number[_startPosition.Length];
        GetPosition(in x, in parameters);
        ValueWithDerivative1 derivativeWithValue = _functionToAnalyse.ValueWithDerivativeByArgumentsChangeDirection(
          arguments: parameters, 
          directionDerivativeRatios: _directionDerivativeRatios);

        return new NumberFunctionPointWithDerivative(
          x: in x,
          y: derivativeWithValue.Value,
          first: derivativeWithValue.First);
      }
  }
}