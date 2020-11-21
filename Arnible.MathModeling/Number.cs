using Arnible.MathModeling.Export;
using System;
using System.Globalization;

namespace Arnible.MathModeling
{
  /// <summary>
  /// Double with relaxed equality operator.
  /// </summary>
  /// <remarks> 
  /// Features:
  /// * DoubleExtension.NumericEquals is used for equality decision  
  /// Usage considerations:
  /// * Structure size is less then IntPtr.Size on 64-bit processes, hence it is suggested to not return/receive structure instance by reference
  /// </remarks>  
  [Serializable]
  public readonly struct Number : 
    IEquatable<Number>, 
    IComparable<Number>, 
    IValueObject
  {    
    private readonly double _value;

    private Number(in double value)
    {
      if (!value.IsValidNumeric())
      {
        throw new ArgumentException($"{nameof(value)}: [{value}]");
      }
      _value = value;
    }

    public static implicit operator Number(in double v) => new Number(in v);
    public static explicit operator double(in Number v) => v._value;
    
    //
    // Serializer
    //

    class Serializer : IRecordWriter<Number>
    {
      private readonly IRecordFieldSerializer _serializer;
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
      }
      public void Write(in Number record)
      {
        _serializer.Write(string.Empty, record._value);
      }

      public void WriteNull()
      {
        _serializer.WriteNull(string.Empty);
      }
    }
    
    public static IRecordWriter<Number> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }

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
    public int GetHashCodeValue() => GetHashCode();
    
    public string ToString(in CultureInfo cultureInfo)
    {
      return _value.ToString(cultureInfo);
    }
    public override string ToString() => ToString(CultureInfo.InvariantCulture);
    public string ToStringValue() => ToString();

    //
    // Operators
    //

    public bool Equals(Number other) => _value.NumericEquals(other._value);

    public bool Equals(in Number other) => _value.NumericEquals(in other._value);

    public static bool operator ==(in Number a, in Number b) => a.Equals(in b);
    public static bool operator !=(in Number a, in Number b) => !a.Equals(in b);

    public static bool operator <(in Number a, in Number b)
    {
      if(a.Equals(in b))
      {
        return false;
      }
      else
      {
        return a._value < b._value;
      }
    }

    public static bool operator >(in Number a, in Number b)
    {
      if (a.Equals(in b))
      {
        return false;
      }
      else
      {
        return a._value > b._value;
      }
    }

    public static bool operator <=(in Number a, in Number b)
    {
      if(a.Equals(in b))
      {
        return true;
      }
      else
      {
        return a._value < b._value;
      }
    }

    public static bool operator >=(in Number a, in Number b)
    {
      if (a.Equals(in b))
      {
        return true;
      }
      else
      {
        return a._value > b._value;
      }
    }

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

    public Number ToPower(in uint b) => DoubleExtension.ToPower(in _value, in b);

    //
    // IComparable
    //

    public int CompareTo(Number other)
    {
      if (_value.NumericEquals(other._value)) return 0;
      return _value > other._value ? 1 : -1;
    }
    
    //
    // Extensions
    //
    public Number Abs()
    {
      return Math.Abs(_value);
    }
  }
}
