using System;
using System.Globalization;
using Arnible.Export;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

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
  public readonly partial struct Number 
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
    // Serializable
    //
    
    public class Serializer : ValueRecordSerializerSimple<Number>
    {
      public override void Serialize(IRecordFieldSerializer serializer, in Number? record)
      {
        serializer.Write(string.Empty, record?._value);
      }
    }

    //
    // Comparision operators
    //

    public bool Equals(in Number other) => _value.NumericEquals(in other._value);
    
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
    
    //
    // IComparable<Number>
    //
    
    public int CompareTo(Number other)
    {
      if (Equals(in other))
      {
        return 0;
      }
      else
      {
        return _value > other._value ? 1 : -1;  
      }
    }
    
    //
    // Arithmetic operators
    //

    public Number ToPower(in uint b) => DoubleExtension.ToPower(in _value, in b);
    
    //
    // Extensions
    //
    
    public Number Abs()
    {
      return Math.Abs(_value);
    }
  }
}
