using System;
using System.Globalization;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling
{
  public readonly partial struct Number :
    IValueEquatable<Number>,
    IComparable<Number>,
    IAlgebraUnitRing<Number>
  {
    static readonly Number _one = 1;
    static readonly Number _zero = 0;
    
    //
    // Object
    //

    public override bool Equals(object? obj)
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

    public override string ToString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }
    
    //
    // IEquatable<Number>
    //
    
    public bool Equals(Number other)
    {
      return Equals(in other);
    }
    
    public static bool operator ==(in Number a, in Number b) => a.Equals(in b);
    public static bool operator !=(in Number a, in Number b) => !a.Equals(in b);
    
    //
    // IAlgebraUnitRing<Number>
    //
    
    public ref readonly Number One => ref _one;
    public ref readonly Number Zero => ref _zero;
    
    public Number Add(in Number component) => this._value + component._value;
    public Number Multiply(in Number factor) => this._value * factor._value;
    
    public Number Inverse() => -1 * this._value;
    
    
    
    //
    // Arithmetic operators
    //
    
    public static Number operator /(in Number a, in Number b) => a._value / b._value;
    public static Number operator /(in Number a, in double b) => a._value / b;
    public static Number operator /(in double a, in Number b) => a / b._value;
    public static Number operator /(in Number a, in int b) => a._value / b;
    public static Number operator /(in int a, in Number b) => a / b._value;
    public static Number operator /(in Number a, in uint b) => a._value / b;
    public static Number operator /(in uint a, in Number b) => a / b._value;
    
    public static Number operator +(in Number a, in Number b) => a._value + b._value;
    public static Number operator +(in Number a, in double b) => a._value + b;
    public static Number operator +(in double a, in Number b) => a + b._value;
    public static Number operator +(in Number a, in int b) => a._value + b;
    public static Number operator +(in int a, in Number b) => a + b._value;
    public static Number operator +(in Number a, in uint b) => a._value + b;
    public static Number operator +(in uint a, in Number b) => a + b._value;


    public static Number operator -(in Number a, in Number b) => a._value - b._value;
    public static Number operator -(in Number a, in double b) => a._value - b;
    public static Number operator -(in double a, in Number b) => a - b._value;
    public static Number operator -(in Number a, in int b) => a._value - b;
    public static Number operator -(in int a, in Number b) => a - b._value;
    public static Number operator -(in Number a, in uint b) => a._value - b;
    public static Number operator -(in uint a, in Number b) => a - b._value;

    public static Number operator *(in Number a, in Number b) => a._value * b._value;
    public static Number operator *(in Number a, in double b) => a._value * b;
    public static Number operator *(in double a, in Number b) => a * b._value;
    public static Number operator *(in Number a, in int b) => a._value * b;
    public static Number operator *(in int a, in Number b) => a * b._value;    
    public static Number operator *(in Number a, in uint b) => a._value * b;
    public static Number operator *(in uint a, in Number b) => a * b._value;  
  }
}