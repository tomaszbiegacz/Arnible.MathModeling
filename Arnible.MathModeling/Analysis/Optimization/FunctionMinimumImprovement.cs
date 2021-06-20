using System;
using Arnible.Assertions;
using Arnible.Export;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public ref struct FunctionMinimumImprovement
  {
    private readonly IFunctionValueAnalysis _function;
    private readonly Span<Number> _parameters;
    private Number _value;
    private uint _complexity;
    private ReadOnlySpan<char> _notes;
      
    public FunctionMinimumImprovement(
      IFunctionValueAnalysis function,
      in ReadOnlySpan<Number> sourceParameters,
      in Span<Number> solutionBuffer)
    {
      solutionBuffer.Length.AssertIsEqualTo(sourceParameters.Length);
      
      _parameters = solutionBuffer;
      sourceParameters.CopyTo(_parameters);
      
      _function = function;
      _value = _function.GetValue(in sourceParameters);
      _complexity = 0;
      _notes = "Start";
    }
    
    public IFunctionValueAnalysis Function => _function;
    public ushort ParametersCount => (ushort)_parameters.Length;
    
    public Span<Number> Parameters => _parameters;
    public Number Value => _value;
    public uint Complexity => _complexity;
    public ReadOnlySpan<char> Notes => _notes;
    
    public bool ConsiderSolution(
      in Number solutionValue,
      uint complexity,
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
    
    //
    // Serializers
    //
    
    public void SerializeCurrentState(
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
