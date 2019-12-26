using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public struct Number
  {
    private Polynomial _term;

    private Number(Polynomial term)
    {
      _term = term;
    }

    public static implicit operator Number(double v) => new Number(v);
    public static implicit operator Number(PolynomialTerm v) => new Number(v);
    public static implicit operator Number(Polynomial v) => new Number(v);

    public static implicit operator Polynomial(Number v) => v._term;

    public static explicit operator double(Number v) => (double)v._term;
    public static explicit operator PolynomialTerm(Number v) => (PolynomialTerm)v._term;

    public static PolynomialTerm Term(uint pos) => (char)('a' + pos);

    public static Number[] Terms(uint pos)
    {
      List<Number> result = new List<Number>();
      for (uint i = 0; i < pos; ++i)
      {
        result.Add(Term(i));
      }
      return result.ToArray();
    }

    public bool IsValidNumeric() => true;

    public static Polynomial operator +(Number a, Number b) => a._term + b._term;
    public static Polynomial operator +(Number a, Polynomial b) => a._term + b;
    public static Polynomial operator +(Polynomial a, Number b) => a + b._term;
    public static Polynomial operator +(Number a, double b) => a._term + b;
    public static Polynomial operator +(double a, Number b) => a + b._term;

    public static Polynomial operator -(Number a, Number b) => a._term - b._term;
    public static Polynomial operator -(Number a, Polynomial b) => a._term - b;
    public static Polynomial operator -(Polynomial a, Number b) => a - b._term;
    public static Polynomial operator -(Number a, double b) => a._term - b;
    public static Polynomial operator -(double a, Number b) => a - b._term;

    public static Polynomial operator *(Number a, Number b) => a._term * b._term;
    public static Polynomial operator *(Number a, Polynomial b) => a._term * b;
    public static Polynomial operator *(Polynomial a, Number b) => a * b._term;
    public static Polynomial operator *(Number a, double b) => a._term * b;
    public static Polynomial operator *(double a, Number b) => a * b._term;

    public Number ToPower(uint b) => _term.ToPower(b);
  }
}
