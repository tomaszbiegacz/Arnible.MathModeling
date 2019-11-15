using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arnible.MathModeling
{
  public struct Polynomial
  {
    private readonly IEnumerable<PolynomialVariable> _variables;

    public Polynomial(params PolynomialVariable[] variables)
      : this((IEnumerable<PolynomialVariable>)variables)
    {
      // intentionally empty
    }

    public Polynomial(IEnumerable<PolynomialVariable> variables)
    {
      _variables = PolynomialVariable.Simplify(variables).ToArray();
    }

    public static implicit operator Polynomial(PolynomialVariable v) => new Polynomial(v);
    public static implicit operator Polynomial(double value) => new Polynomial(value);
    public static implicit operator Polynomial(char name) => new Polynomial(name);

    private IEnumerable<PolynomialVariable> Variables => _variables ?? Enumerable.Empty<PolynomialVariable>();

    public bool Equals(Polynomial other) => (other - this).IsZero;

    public override int GetHashCode()
    {
      int result = 1;
      foreach (var v in Variables)
      {
        result *= v.GetHashCode();
      }
      return result;
    }

    public override bool Equals(object obj)
    {
      if (obj is Polynomial v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override string ToString()
    {
      if(IsZero)
      {
        return "0";
      }
      else
      {
        var result = new StringBuilder();
        bool printOperator = false;
        foreach(var variable in Variables)
        {
          if(printOperator && variable.IsPositive)
          {
            result.Append("+");
          }
          result.Append(variable.ToString());

          printOperator = true;
        }
        return result.ToString();
      }      
    }

    public static bool operator ==(Polynomial a, Polynomial b) => a.Equals(b);
    public static bool operator !=(Polynomial a, Polynomial b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsZero => !Variables.Any();

    /*
     * Operators
     */

    private static IEnumerable<PolynomialVariable> MultiplyVariables(Polynomial a, Polynomial b)
    {
      foreach (var v1 in a.Variables)
      {
        foreach (var v2 in b.Variables)
        {
          yield return v1 * v2;
        }
      }
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
      return new Polynomial(MultiplyVariables(a, b));
    }

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
      return new Polynomial(a.Variables.Concat(b.Variables));
    }

    public static Polynomial operator -(Polynomial a, Polynomial b)
    {
      return new Polynomial(a.Variables.Concat(b.Variables.Select(v => -1 * v)));
    }

    public Polynomial DerivativeBy(char name)
    {
      return new Polynomial(Variables.Select(v => v.DerivativeBy(name)));
    }
  }
}
