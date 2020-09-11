using Arnible.MathModeling.Polynomials;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling
{
  public readonly struct Number : 
    IEquatable<Number>, 
    IComparable<Number>, 
    IValueObject
  {
    private readonly PolynomialDivision _value;

    private Number(in PolynomialDivision value)
    {
      _value = value;
    }

    private Number(in Polynomial value)
    {
      _value = value;
    }

    public static implicit operator Number(in double v) => new Number(v);
    public static explicit operator double(in Number v) => (double)v._value;

    public static implicit operator Number(in PolynomialTerm v) => new Number(v);
    public static explicit operator PolynomialTerm(in Number v) => (PolynomialTerm)v._value;

    public static implicit operator Number(in Polynomial v) => new Number(in v);
    public static explicit operator Polynomial(in Number v) => (Polynomial)v._value;

    public static implicit operator Number(in PolynomialDivision v) => new Number(v);
    public static explicit operator PolynomialDivision(in Number v) => v._value;

    //
    // Object
    //

    public override bool Equals(object obj)
    {
      if (obj is Number v)
      {
        return Equals(in v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return _value.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public string ToString(CultureInfo cultureInfo)
    {
      return _value.ToString(cultureInfo);
    }
    public override string ToString() => ToString(CultureInfo.InvariantCulture);
    public string ToStringValue() => ToString();

    //
    // Operators
    //

    public bool Equals(in Number other) => _value == other._value;

    public bool Equals(Number other) => Equals(in other);

    public static bool operator ==(in Number a, in Number b) => a.Equals(b);
    public static bool operator !=(in Number a, in Number b) => !a.Equals(b);    

    public static bool operator >(in Number a, in Number b)
    {
      var result = a._value - b._value;
      if (result.IsConstant)
      {
        return (double)result > 0;
      }
      else
      {
        return false;
      }
    }
    public static bool operator <(in Number a, in Number b) => b > a;

    public static bool operator >=(in Number a, in Number b) => a > b || a == b;
    public static bool operator <=(in Number a, in Number b) => a < b || a == b;
    

    public static Number operator /(in Number a, in Number b) => a._value / b._value;
    public static Number operator /(in Number a, in double b) => a._value / b;
    public static Number operator /(in double a, in Number b) => a / b._value;
    public static Number operator /(in Number a, in int b) => a._value / b;
    public static Number operator /(in int a, in Number b) => a / b._value;
    public static Number operator /(in Number a, in uint b) => a._value / b;
    public static Number operator /(in uint a, in Number b) => a / b._value;    
    public static Number operator /(in Number a, in Polynomial b) => a._value / b;
    public static Number operator /(in Polynomial a, in Number b) => a / b._value;    


    public static Number operator +(in Number a, in Number b) => a._value + b._value;
    public static Number operator +(in Number a, in double b) => a._value + b;
    public static Number operator +(in double a, in Number b) => a + b._value;
    public static Number operator +(in Number a, in int b) => a._value + b;
    public static Number operator +(in int a, in Number b) => a + b._value;
    public static Number operator +(in Number a, in uint b) => a._value + b;
    public static Number operator +(in uint a, in Number b) => a + b._value;    
    public static Number operator +(in Number a, in Polynomial b) => a._value + b;
    public static Number operator +(in Polynomial a, in Number b) => a + b._value;    


    public static Number operator -(in Number a, in Number b) => a._value - b._value;
    public static Number operator -(in Number a, in double b) => a._value - b;
    public static Number operator -(in double a, in Number b) => a - b._value;
    public static Number operator -(in Number a, in int b) => a._value - b;
    public static Number operator -(in int a, in Number b) => a - b._value;
    public static Number operator -(in Number a, in uint b) => a._value - b;
    public static Number operator -(in uint a, in Number b) => a - b._value;    
    public static Number operator -(in Number a, in Polynomial b) => a._value - b;
    public static Number operator -(in Polynomial a, in Number b) => a - b._value;
    

    public static Number operator *(in Number a, in Number b) => a._value * b._value;
    public static Number operator *(in Number a, in double b) => a._value * b;
    public static Number operator *(in double a, in Number b) => a * b._value;
    public static Number operator *(in Number a, in int b) => a._value * b;
    public static Number operator *(in int a, in Number b) => a * b._value;    
    public static Number operator *(in Number a, in uint b) => a._value * b;
    public static Number operator *(in uint a, in Number b) => a * b._value;        
    public static Number operator *(in Number a, in Polynomial b) => a._value * b;
    public static Number operator *(in Polynomial a, in Number b) => a * b._value;    


    public Number ToPower(in uint b) => _value.ToPower(in b);

    //
    // IComparable
    //

    public int CompareTo(Number other)
    {
      if (_value == other._value) return 0;

      double result = (double)(_value - other._value);
      return result > 0 ? 1 : -1;
    }

    //
    // Term
    //

    public static PolynomialTerm Term(uint pos) => pos < 26 ? (char)('a' + pos) : throw new ArgumentException($"To big pos: {pos}");

    public static PolynomialTerm GreekTerm(uint pos) => pos < 24 ? (char)('α' + pos) : throw new ArgumentException($"To big pos: {pos}");

    private static IEnumerable<Number> Terms(uint pos, Func<uint, PolynomialTerm> termFactory)
    {
      for (uint i = 0; i < pos; ++i)
      {
        yield return termFactory(i);
      }
    }

    public static IEnumerable<Number> Terms(in uint pos) => Terms(pos, Term);

    public static IEnumerable<Number> GreekTerms(in uint pos) => Terms(pos, GreekTerm);    
  }
}
