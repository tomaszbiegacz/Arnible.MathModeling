using System;

namespace Arnible.MathModeling.Analysis
{
  interface IDerivative1
  {
    Number First { get; }
  }

  public readonly struct Derivative1Value : IDerivative1, IValueEquatable<Derivative1Value>
  {
    public Derivative1Value(in Number first)
    {
      First = first;
    }
    
    public static explicit operator Derivative1Value(in Number v) => new Derivative1Value(in v);

    //
    // Properties
    //

    public Number First { get; }
    
    //
    // IEquatable
    // 
    //
    
    public bool Equals(Derivative1Value other) => Equals(in other);

    public bool Equals(in Derivative1Value other)
    {
      return First == other.First;
    }

    public override bool Equals(object? obj)
    {
      return obj is Derivative1Value other && Equals(other);
    }

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }

    public override string ToString()
    {
      return First.ToString();
    }
    
    public static bool operator ==(in Derivative1Value a, in Derivative1Value b) => a.Equals(in b);
    public static bool operator !=(in Derivative1Value a, in Derivative1Value b) => !a.Equals(in b);

    //
    // 
    // IEquatable
    //
  }
}
