using System;
using Arnible.Assertions;
using Arnible.Export;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Geometry;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public ref struct FunctionMinimumImprovement
  {
    private readonly IFunctionValueAnalysis _function;
    private readonly Span<Number> _parameters;
    private readonly ConjugateDirection _conjugateDirection;
    private Number _value;
    private ulong _complexity;
    private ReadOnlySpan<char> _notes;

    public FunctionMinimumImprovement(
      IFunctionValueAnalysis function,
      in ReadOnlySpan<Number> sourceParameters,
      in Span<Number> solutionBuffer,
      in ConjugateDirection conjugateDirection)
    {
      solutionBuffer.Length.AssertIsEqualTo(sourceParameters.Length);
      if(conjugateDirection.DirectionsMemorySize > 0)
      {
        conjugateDirection.DirectionsDimensionsCount.AssertIsEqualTo(sourceParameters.Length);
      }
      
      _parameters = solutionBuffer;
      _conjugateDirection = conjugateDirection;
      sourceParameters.CopyTo(_parameters);
      
      _function = function;
      _value = _function.GetValue(in sourceParameters);
      _complexity = 0;
      _notes = "Start";
    }
    
    public FunctionMinimumImprovement(
      IFunctionValueAnalysis function,
      in ReadOnlySpan<Number> sourceParameters,
      in Span<Number> solutionBuffer)
      : this(function, in sourceParameters, in solutionBuffer, default)
    {
      // intentionally empty
    }
    
    public readonly IFunctionValueAnalysis Function => _function;
    public readonly ushort ParametersCount => (ushort)_parameters.Length;
    
    public readonly Span<Number> Parameters => _parameters;
    public readonly Number Value => _value;
    public readonly ulong Complexity => _complexity;
    public readonly ReadOnlySpan<char> Notes => _notes;
    
    public bool ConsiderSolution(
      in Number solutionValue,
      in ulong complexity,
      in ReadOnlySpan<char> notes)
    {
      if(solutionValue.PreciselySmaller(_value))
      {
        _value = solutionValue;
        _complexity = complexity;
        _notes = notes;
        return true;
      }
      else
      {
        return false;
      }
    }
    
    public readonly bool HasConjugateDirections => _conjugateDirection.HasConjugateDirections;
    
    public readonly void GetConjugateDirection(in Span<Number> direction) => _conjugateDirection.GetConjugateDirection(in direction);
    
    public readonly void FinaliseCurrentDirectionSearch(in ReadOnlySpan<Number> startingPoint)
    {
      if(_conjugateDirection.DirectionsMemorySize > 0)
      {
        Span<Number> direction = stackalloc Number[ParametersCount];
        startingPoint.CopyTo(direction);
        direction.MultiplySelfBy(-1);
        direction.AddToSelf(_parameters);
        
        _conjugateDirection.AddDirection(direction);
      }
    }
    
    //
    // Serializers
    //
    
    public readonly void SerializeCurrentState(
      IRecordFieldSerializer serializer, 
      uint pos,
      ulong totalComplexity,
      bool withExtendedSearch)
    {
      Span<Number> gradient = stackalloc Number[Parameters.Length];
      Function.GradientByArguments(Parameters, in gradient);
      
      serializer.Write("Pos", pos);
      serializer.CollectionField<Number>().Write(nameof(Parameters), Parameters);
      serializer.WriteValueField(nameof(Value), Value);
      serializer.Write(nameof(Complexity), Complexity);
      serializer.Write(nameof(Notes), Notes);
      serializer.Write("WithExtendedSearch", withExtendedSearch);
      serializer.Write("TotalComplexity", totalComplexity);
      serializer.CollectionField<Number>().Write("Gradient", gradient);
    }
  }
}
