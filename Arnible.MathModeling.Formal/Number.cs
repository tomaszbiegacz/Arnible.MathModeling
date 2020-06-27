using System;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling
{
  public readonly struct Number : IEquatable<Number>, IComparable<Number>
  {
    private readonly PolynomialDivision _value;

    private Number(PolynomialDivision value)
    {
      _value = value;
    }

    private Number(Polynomial value)
    {
      _value = value;
    }

    public static implicit operator Number(double v) => new Number(v);
    public static explicit operator double(Number v) => (double)v._value;

    public static implicit operator Number(PolynomialTerm v) => new Number(v);
    public static explicit operator PolynomialTerm(Number v) => (PolynomialTerm)v._value;

    public static implicit operator Number(Polynomial v) => new Number(v);
    public static explicit operator Polynomial(Number v) => (Polynomial)v._value;

    public static implicit operator Number(PolynomialDivision v) => new Number(v);
    public static explicit operator PolynomialDivision(Number v) => v._value;

    //
    // Object
    //

    public override bool Equals(object obj)
    {
      if (obj is Number v)
      {
        return Equals(v);
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

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      return _value.ToString(cultureInfo);
    }

    //
    // Operators
    //

    public bool Equals(Number other) => _value == other._value;

    public static bool operator ==(Number a, Number b) => a.Equals(b);
    public static bool operator !=(Number a, Number b) => !a.Equals(b);    

    public static bool operator >(Number a, Number b)
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
    public static bool operator <(Number a, Number b) => b > a;

    public static bool operator >=(Number a, Number b) => a > b || a == b;
    public static bool operator <=(Number a, Number b) => a < b || a == b;
    

    public static Number operator /(Number a, Number b) => a._value / b._value;
    public static Number operator /(Number a, double b) => a._value / b;
    public static Number operator /(double a, Number b) => a / b._value;
    public static Number operator /(Number a, int b) => a._value / b;
    public static Number operator /(int a, Number b) => a / b._value;
    public static Number operator /(Number a, uint b) => a._value / b;
    public static Number operator /(uint a, Number b) => a / b._value;    
    public static Number operator /(Number a, Polynomial b) => a._value / b;
    public static Number operator /(Polynomial a, Number b) => a / b._value;    


    public static Number operator +(Number a, Number b) => a._value + b._value;
    public static Number operator +(Number a, double b) => a._value + b;
    public static Number operator +(double a, Number b) => a + b._value;
    public static Number operator +(Number a, int b) => a._value + b;
    public static Number operator +(int a, Number b) => a + b._value;
    public static Number operator +(Number a, uint b) => a._value + b;
    public static Number operator +(uint a, Number b) => a + b._value;    
    public static Number operator +(Number a, Polynomial b) => a._value + b;
    public static Number operator +(Polynomial a, Number b) => a + b._value;    


    public static Number operator -(Number a, Number b) => a._value - b._value;
    public static Number operator -(Number a, double b) => a._value - b;
    public static Number operator -(double a, Number b) => a - b._value;
    public static Number operator -(Number a, int b) => a._value - b;
    public static Number operator -(int a, Number b) => a - b._value;
    public static Number operator -(Number a, uint b) => a._value - b;
    public static Number operator -(uint a, Number b) => a - b._value;    
    public static Number operator -(Number a, Polynomial b) => a._value - b;
    public static Number operator -(Polynomial a, Number b) => a - b._value;
    

    public static Number operator *(Number a, Number b) => a._value * b._value;
    public static Number operator *(Number a, double b) => a._value * b;
    public static Number operator *(double a, Number b) => a * b._value;
    public static Number operator *(Number a, int b) => a._value * b;
    public static Number operator *(int a, Number b) => a * b._value;    
    public static Number operator *(Number a, uint b) => a._value * b;
    public static Number operator *(uint a, Number b) => a * b._value;        
    public static Number operator *(Number a, Polynomial b) => a._value * b;
    public static Number operator *(Polynomial a, Number b) => a * b._value;    


    public Number ToPower(uint b) => _value.ToPower(b);

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

    public static IEnumerable<Number> Terms(uint pos) => Terms(pos, Term);

    public static IEnumerable<Number> GreekTerms(uint pos) => Terms(pos, GreekTerm);    
  }
}
