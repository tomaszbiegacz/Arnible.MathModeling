using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  class PolynomialFinitaryOperation : IFinitaryOperation<double>
  {
    private readonly IPolynomialOperation _polynomial;
    private readonly ValueArray<char> _variables;

    public PolynomialFinitaryOperation(IPolynomialOperation polynomial, IEnumerable<char> variables)
    {
      _polynomial = polynomial ?? throw new ArgumentNullException(nameof(polynomial));

      var polynomialVariables = new HashSet<char>(_polynomial.Variables);
      if (!polynomialVariables.SetEquals(variables))
      {
        throw new ArgumentException($"variables are not equal to polynomial variables");
      }
      _variables = variables.ToValueArray();
    }

    public double Value(IEnumerable<double> x)
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
            throw new ArgumentException($"Too many arguments, expected {_variables.Length}, got {i}.");
          }
          args[_variables[(uint)i]] = xEnum.Current;
        }
        if (i + 1 < _variables.Length)
        {
          throw new ArgumentException($"too few arguments, expected {_variables.Length}, got {i}.");
        }
      }        
      return _polynomial.Value(args);
    }
  }
}
