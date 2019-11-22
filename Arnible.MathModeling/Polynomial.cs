using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arnible.MathModeling
{
  public struct Polynomial : IEquatable<PolynomialDivision>, IEquatable<Polynomial>, IPolynomialOperation
  {
    private readonly IEnumerable<PolynomialTerm> _terms;

    internal Polynomial(params PolynomialTerm[] terms)
      : this((IEnumerable<PolynomialTerm>)terms)
    {
      // intentionally empty
    }

    public Polynomial(IEnumerable<PolynomialTerm> terms)
    {
      _terms = PolynomialTerm.Simplify(terms).ToArray();
    }

    public static implicit operator Polynomial(PolynomialTerm v) => new Polynomial(v);
    public static implicit operator Polynomial(double value) => new Polynomial(value);
    public static implicit operator Polynomial(char name) => new Polynomial(name);

    private IEnumerable<PolynomialTerm> Terms => _terms ?? Enumerable.Empty<PolynomialTerm>();

    public bool Equals(Polynomial other) => (other - this).IsZero;

    public bool Equals(PolynomialDivision other) => other.IsPolynomial ? Equals((Polynomial)other) : false;

    public override int GetHashCode()
    {
      int result = 1;
      foreach (var v in Terms)
      {
        result *= v.GetHashCode();
      }
      return result;
    }

    public override bool Equals(object obj)
    {
      if (obj is PolynomialDivision pd)
      {
        return Equals(pd);
      }
      else if (obj is Polynomial v)
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
      if (IsZero)
      {
        return "0";
      }
      else
      {
        var result = new StringBuilder();
        bool printOperator = false;
        foreach (var variable in Terms)
        {
          if (printOperator && variable.HasPositiveCoefficient)
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

    public bool IsZero => !Terms.Any();

    public bool HasOneTerm => Terms.Count() < 2;

    public bool IsConstant
    {
      get
      {
        if (!HasOneTerm)
          return false;
        else
          return Terms.SingleOrDefault().IsConstant;
      }
    }

    /*
     * Operators
     */

    public static explicit operator PolynomialTerm(Polynomial v) => v.Terms.SingleOrDefault();

    public static explicit operator double(Polynomial v) => (double)v.Terms.SingleOrDefault();

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
      return new Polynomial(a.Terms.Concat(b.Terms));
    }

    public static Polynomial operator -(Polynomial a, Polynomial b)
    {
      return new Polynomial(a.Terms.Concat(b.Terms.Select(v => -1 * v)));
    }

    private static IEnumerable<PolynomialTerm> MultiplyVariables(Polynomial a, Polynomial b)
    {
      foreach (var v1 in a.Terms)
      {
        foreach (var v2 in b.Terms)
        {
          yield return v1 * v2;
        }
      }
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
      return new Polynomial(MultiplyVariables(a, b));
    }

    public static Polynomial operator *(PolynomialTerm a, Polynomial b)
    {
      return new Polynomial(b.Terms.Select(t => a * t));
    }

    public static Polynomial operator *(Polynomial b, PolynomialTerm a) => a * b;

    public Polynomial Power(uint power)
    {
      switch (power)
      {
        case 0:
          return 1;
        case 1:
          return this;
        default:
          return this * this.Power(power - 1);
      }
    }

    public static PolynomialDivision operator /(Polynomial a, Polynomial b)
    {
      return new PolynomialDivision(a, b);
    }

    public static Polynomial operator /(Polynomial a, double denominator)
    {
      return new Polynomial(a.Terms.Select(t => t / denominator));
    }

    private static Polynomial TryReduce(
      Polynomial a,
      PolynomialTerm denominator,
      Polynomial denominatorSuffix,
      List<PolynomialTerm> result)
    {
      if (a == 0)
      {
        // we are done, there is nothing more to extract from
        return 0;
      }

      foreach (PolynomialTerm aTerm in a.Terms)
      {
        if (aTerm.TryDivide(denominator, out PolynomialTerm remainder))
        {
          result.Add(remainder);

          // let's see what is left and continue reducing on it
          Polynomial remaining = a - aTerm - denominatorSuffix * remainder;
          return TryReduce(remaining, denominator, denominatorSuffix, result);
        }
      }

      // we weren't able to reduce it
      return a;
    }

    public Polynomial ReduceBy(Polynomial b, out Polynomial reminder)
    {
      if (b.IsConstant)
      {
        reminder = default;
        return this / (double)b;
      }
      if (IsZero)
      {
        reminder = default;
        return 0;
      }

      PolynomialTerm denominator = b.Terms.First();
      Polynomial denominatorSuffix = new Polynomial(b.Terms.Skip(1));
      var resultTerms = new List<PolynomialTerm>();
      reminder = TryReduce(this, denominator, denominatorSuffix, resultTerms);
      return new Polynomial(resultTerms);
    }

    public static Polynomial operator %(Polynomial a, Polynomial b)
    {
      a.ReduceBy(b, out Polynomial reminder);
      return reminder;
    }

    public Polynomial ReduceBy(Polynomial b)
    {
      Polynomial result = ReduceBy(b, out Polynomial reminder);
      if (reminder != 0)
      {
        throw new InvalidOperationException($"Cannot reduce [{this}] with [{b}].");
      }
      return result;
    }

    public Polynomial DerivativeBy(char name)
    {
      return new Polynomial(Terms.Select(v => v.DerivativeBy(name)));
    }

    public Polynomial DerivativeBy(PolynomialTerm name) => DerivativeBy((char)name);

    public Polynomial Composition(char variable, Polynomial replacement)
    {
      List<PolynomialTerm> remaining = new List<PolynomialTerm>();

      Polynomial result = 0;
      foreach (PolynomialTerm term in Terms)
      {
        if (term.Variables.Contains(variable))
        {
          result += term.Composition(variable, replacement);
        }
        else
        {
          remaining.Add(term);
        }
      }

      if (remaining.Count > 0)
      {
        result += new Polynomial(remaining);
      }

      return result;
    }

    public PolynomialDivision Composition(char variable, PolynomialDivision replacement)
    {
      List<PolynomialTerm> remaining = new List<PolynomialTerm>();

      PolynomialDivision result = PolynomialDivision.Zero;
      foreach (PolynomialTerm term in Terms)
      {
        if (term.Variables.Contains(variable))
        {
          result += term.Composition(variable, replacement);
        }
        else
        {
          remaining.Add(term);
        }
      }

      if (remaining.Count > 0)
      {
        result += new Polynomial(remaining);
      }

      return result;
    }

    public Polynomial Composition(PolynomialTerm variable, Polynomial replacement) => Composition((char)variable, replacement);

    public PolynomialDivision Composition(PolynomialTerm variable, PolynomialDivision replacement) => Composition((char)variable, replacement);

    /*
     * IPolynomialOperation
     */

    public IEnumerable<char> Variables => Terms.SelectMany(kv => kv.Variables);

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (IsZero)
      {
        return 0;
      }
      else
      {
        return _terms.Select(t => t.Value(x)).Sum();
      }
    }
  }
}
