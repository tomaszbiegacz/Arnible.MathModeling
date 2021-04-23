using System;
using System.Collections.Generic;
using Arnible.Assertions;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  record PolynomialFinitaryOperation : IFinitaryOperation<double>
  {
    private readonly Func<IReadOnlyDictionary<char, double>, double> _valueCalculation;
    private readonly ReadOnlyArray<PolynomialTerm> _variables;

    public PolynomialFinitaryOperation(
      ReadOnlyArray<PolynomialTerm> variables,
      Func<IReadOnlyDictionary<char, double>, double> valueCalculation)
    {
      _variables = variables;
      _valueCalculation = valueCalculation;
    }

    public double Value(in ReadOnlySpan<double> x)
    {
      x.AssertLengthEqualsTo(_variables.Length);
      
      var args = new Dictionary<char, double>();
      for(ushort i=0; i<_variables.Length; ++i)
      {
        args[(char)_variables[i]] = x[i];
        
      }

      return _valueCalculation(args);
    }
  }
}