﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Arnible.Linq;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  /// <summary>
  /// Polynomial representation as a list of polynomial terms.
  /// Size IntPtr, there is no need to pass it via "in".
  /// </summary>
  public readonly struct Polynomial :
    IEquatable<PolynomialDivision>,
    IEnumerable<PolynomialTerm>,
    IPolynomialOperation,
    IAlgebraUnitRing<Polynomial>
  {
    static readonly Polynomial _one = 1;
    static readonly Polynomial _zero = 0;
    
    const char InPlaceVariableReplacement = '$';

    private readonly ReadOnlyArray<PolynomialTerm> _terms;

    internal Polynomial(params PolynomialTerm[] terms)
    {
      _terms = PolynomialTerm.Simplify(terms);
    }

    private Polynomial(ReadOnlyArray<PolynomialTerm> terms)
    {
      _terms = terms;
    }

    private Polynomial(double v)
    {
      if (v.NumericEquals(0))
      {
        _terms = default;
      }
      else
      {
        _terms = new PolynomialTerm[] {v};
      }
    }

    private Polynomial(char v)
    {
      _terms = new PolynomialTerm[] {v};
    }

    private static Polynomial CreateSimplified(IEnumerable<PolynomialTerm> terms)
    {
      return new Polynomial(PolynomialTerm.Simplify(terms));
    }

    public static implicit operator Polynomial(PolynomialTerm v) => new Polynomial(v);
    public static implicit operator Polynomial(double value) => new Polynomial(value);
    public static implicit operator Polynomial(char name) => new Polynomial(name);

    public bool Equals(Polynomial other)
    {
      return _terms.Equals(other._terms);
    }

    public override int GetHashCode()
    {
      return _terms.GetHashCode();
    }

    public bool Equals(PolynomialDivision other) => other.IsPolynomial && Equals((Polynomial) other);

    public override bool Equals(object? obj)
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
    
    

    public string ToString(CultureInfo cultureInfo)
    {
      if (_terms.Length == 0)
      {
        return "0";
      }
      else
      {
        var result = new StringBuilder();
        bool printOperator = false;
        foreach (var variable in _terms)
        {
          if (printOperator && variable.HasPositiveCoefficient)
          {
            result.Append("+");
          }

          result.Append(variable.ToString(cultureInfo));

          printOperator = true;
        }

        return result.ToString();
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public static bool operator ==(Polynomial a, Polynomial b) => a.Equals(b);
    public static bool operator !=(Polynomial a, Polynomial b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsSingleTerm
    {
      get { return _terms.Length <= 1; }
    }

    public bool IsConstant
    {
      get
      {
        if (_terms.Length == 0)
          return true;
        else if (_terms.Length == 1)
          return _terms[0].IsConstant;
        else
          return false;
      }
    }

    /*
     * Query
     */

    internal Dictionary<char, ushort> GetIdentityVariableTerms()
    {
      return _terms.AsList().Select(t => t.GetIdentityVariableTerms())
        .AggregateCommonBy(v => v.Variable, vc => vc.Select(v => v.Power).MinDefensive());
    }

    /*
     * Operators
     */
    
    public ref readonly Polynomial One => ref _one;
    public ref readonly Polynomial Zero => ref _zero;

    public static explicit operator PolynomialTerm(Polynomial v) => v._terms.AsList().SingleOrNone() ?? 0;

    public static explicit operator double(Polynomial v) => (double) (v._terms.AsList().SingleOrNone() ?? 0);

    // +

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
      return CreateSimplified(a._terms.AsList().Concat(b._terms.AsList()));
    }

    public static Polynomial operator +(Polynomial a, double b)
    {
      return CreateSimplified(a._terms.AsList().Append(b));
    }

    public static Polynomial operator +(double a, Polynomial b) => b + a;
    
    public Polynomial Add(in Polynomial component) => this + component;
    
    public Polynomial Inverse() => -1 * this;

    // -

    public static Polynomial operator -(Polynomial a, Polynomial b)
    {
      return CreateSimplified(a._terms.AsList().Concat(b._terms.AsList().Select(v => -1 * v)));
    }

    public static Polynomial operator -(Polynomial a, double b)
    {
      return CreateSimplified(a._terms.AsList().Append(-1 * b));
    }

    public static Polynomial operator -(double a, Polynomial b) => -1 * b + a;

    // *

    private static IEnumerable<PolynomialTerm> MultiplyVariables(Polynomial a, Polynomial b)
    {
      foreach (var v1 in a)
      {
        foreach (var v2 in b)
        {
          yield return v1 * v2;
        }
      }
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
      return CreateSimplified(MultiplyVariables(a, b));
    }

    public static Polynomial operator *(PolynomialTerm a, Polynomial b)
    {
      if (a == 0)
      {
        return 0;
      }
      else
      {
        // no need for simplification
        return new Polynomial(b._terms.AsList().Select(t => a * t).ToArray());
      }
    }

    public static Polynomial operator *(Polynomial b, PolynomialTerm a) => a * b;

    public static Polynomial operator *(double a, Polynomial b)
    {
      if (a.NumericEquals(0))
      {
        return 0;
      }
      else
      {
        // no need for simplification
        return new Polynomial(b._terms.AsList().Select(t => a * t).ToArray());
      }
    }

    public static Polynomial operator *(Polynomial b, double a) => a * b;
    
    public Polynomial Multiply(in Polynomial factor) => this * factor;

    // /

    public static PolynomialDivision operator /(Polynomial a, Polynomial b)
    {
      return PolynomialDivision.SimplifyPolynomialDivision(a, b);
    }

    public static Polynomial operator /(Polynomial a, double denominator) => a * (1 / denominator);

    public static PolynomialDivision operator /(double a, Polynomial denominator)
    {
      return PolynomialDivision.SimplifyPolynomialDivision(a, denominator);
    }

    /*
     * Other operators
     */

    public Polynomial ToPower(uint power)
    {
      switch (power)
      {
        case 0:
          return 1;
        case 1:
          return this;
        default:
        {
          uint power2 = power / 2;
          var power2Item = ToPower(power2);
          var power2Item2 = power2Item * power2Item;
          if (power % 2 == 0)
            return power2Item2;
          else
            return power2Item2 * this;
        }
      }
    }

    internal Polynomial ReduceByCommon(IEnumerable<VariableTerm> terms)
    {
      // no need to simplify
      return new Polynomial(_terms.AsList().Select(t => t.ReduceByCommon(terms)).ToArray());
    }

    public static Polynomial operator %(Polynomial a, Polynomial b)
    {
      a.DivideBy(b, out Polynomial reminder);
      return reminder;
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

      foreach (PolynomialTerm aTerm in a)
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

    public Polynomial DivideBy(Polynomial b, out Polynomial reminder)
    {
      if (b.IsConstant)
      {
        reminder = default;
        return this / (double) b;
      }

      if (_terms.Length == 0)
      {
        reminder = default;
        return 0;
      }

      PolynomialTerm denominator = b._terms.AsList().First();
      Polynomial denominatorSuffix =
        new Polynomial(b._terms.AsList().SkipExactly(1).ToArray()); // no need for simplification
      var resultTerms = new List<PolynomialTerm>();
      reminder = TryReduce(this, denominator, denominatorSuffix, resultTerms);
      return new Polynomial(resultTerms.ToArray()); // no need for simplification
    }

    public bool TryDivideBy(Polynomial b, out Polynomial result)
    {
      result = DivideBy(b, out Polynomial reminder);
      return reminder == 0;
    }

    public Polynomial DivideBy(Polynomial b)
    {
      Polynomial result = DivideBy(b, out Polynomial reminder);
      if (reminder != 0)
      {
        throw new InvalidOperationException($"Cannot divide [{this}] by [{b}].");
      }

      return result;
    }

    /*
     * Derivative
     */

    public Polynomial DerivativeBy(char name)
    {
      return CreateSimplified(_terms.AsList().SelectMany(v => v.DerivativeBy(name)));
    }

    public Polynomial DerivativeBy(PolynomialTerm name) => DerivativeBy((char) name);

    /*
     * Composition
     */

    private IEnumerable<PolynomialTerm> CompositionIngredients(char variable, Polynomial replacement)
    {
      foreach (PolynomialTerm term in _terms)
      {
        if (term.Variables.Any(v => v == variable))
        {
          foreach (var replacedTerm in term.Composition(variable, replacement))
          {
            yield return replacedTerm;
          }
        }
        else
        {
          yield return term;
        }
      }
    }

    public Polynomial Composition(char variable, Polynomial replacement)
    {
      if (replacement.Variables.Any(v => v == variable))
      {
        if (variable == InPlaceVariableReplacement)
        {
          throw new InvalidOperationException("Something went wrong with in-place variables replacement.");
        }

        // special case for in-place variables replacement
        Polynomial temporaryReplacement = replacement.Composition(variable, InPlaceVariableReplacement);
        return Composition(variable, temporaryReplacement).Composition(InPlaceVariableReplacement, variable);
      }

      return CreateSimplified(CompositionIngredients(variable, replacement));
    }

    public PolynomialDivision Composition(char variable, PolynomialDivision replacement)
    {
      List<PolynomialTerm> remaining = new List<PolynomialTerm>();

      PolynomialDivision result = 0;
      foreach (PolynomialTerm term in _terms)
      {
        if (term.Variables.Any(v => v == variable))
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
        // no need for simplification
        result += new Polynomial(remaining.ToArray());
      }

      return result;
    }

    public Polynomial Composition(PolynomialTerm variable, Polynomial replacement) =>
      Composition((char) variable, replacement);

    public PolynomialDivision Composition(PolynomialTerm variable, PolynomialDivision replacement) =>
      Composition((char) variable, replacement);

    /*
     * IPolynomialOperation
     */

    public IEnumerable<char> Variables => _terms.AsList().SelectMany(kv => kv.Variables);

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (_terms.Length == 0)
      {
        return 0;
      }
      else
      {
        return _terms.AsList().Select(t => t.Value(x)).SumDefensive();
      }
    }

    /*
     * IEnumerator<PolynomialTerm>
     */

    public IEnumerator<PolynomialTerm> GetEnumerator() => _terms.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _terms.GetEnumerator();
  }
}