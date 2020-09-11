using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Polynomials
{
  class PolynomialFinitaryOperation : IFinitaryOperation<double>
  {
    private readonly Func<IReadOnlyDictionary<char, double>, double> _valueCalculation;
    private readonly UnmanagedArray<char> _variables;

    public PolynomialFinitaryOperation(
      in IEnumerable<char> variables,
      in Func<IReadOnlyDictionary<char, double>, double> valueCalculation)
    {
      _variables = variables.ToUnmanagedArray();
      _valueCalculation = valueCalculation;
    }

    public double Value(in IEnumerable<double> x)
    {
      // possibly move it at class level to reduce GC at the cost of thread safety
      var args = new Dictionary<char, double>();

      using (var xEnum = x.GetEnumerator())
      {
        int i = -1;
        while (xEnum.MoveNext())
        {
          i++;
          if (i >= _variables.Length)
          {
            throw new ArgumentException(
              $"Too many arguments, expected {_variables.Length.ToString()}, got {i.ToString()}.");
          }

          args[_variables[(uint) i]] = xEnum.Current;
        }

        if (i + 1 < _variables.Length)
        {
          throw new ArgumentException(
            $"too few arguments, expected {_variables.Length.ToString()}, got {i.ToString()}.");
        }
      }

      return _valueCalculation(args);
    }
  }
}