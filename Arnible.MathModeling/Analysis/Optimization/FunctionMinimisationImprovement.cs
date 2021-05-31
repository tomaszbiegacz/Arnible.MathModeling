﻿using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public readonly ref struct FunctionMinimisationImprovement
  {
    private readonly Span<Number> _parameters;
      
    public FunctionMinimisationImprovement(
      IFunctionValueAnalysis function,
      in ReadOnlySpan<Number> sourceParameters,
      in Span<Number> solutionBuffer)
    {
      sourceParameters.Length.AssertIsEqualTo(solutionBuffer.Length);
      
      Function = function;
      SourceParameters = sourceParameters;
      SourceValue = function.GetValue(in sourceParameters);
      
      _parameters = solutionBuffer;
      sourceParameters.CopyTo(_parameters);
    }
    
    public IFunctionValueAnalysis Function { get; }
    
    public ReadOnlySpan<Number> SourceParameters { get;}
    public Number SourceValue { get; }
    
    public ReadOnlySpan<Number> Parameters => _parameters;
    public Number GetValue() => Function.GetValue(_parameters);
    
    public Number GetRelativeImprovement() => (SourceValue - GetValue()) / SourceValue;
    
    public bool IsNewFound => !Parameters.SequenceEqual(SourceParameters);
    
    public void SetSolution(in ReadOnlySpan<Number> solution)
    {
      solution.CopyTo(_parameters);
    }
    
    public bool ConsiderSolution(in ReadOnlySpan<Number> solution)
    {
      Number currentValue = GetValue();
      Number value = Function.GetValue(in solution);
      if(value < currentValue)
      {
        solution.CopyTo(_parameters);
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
