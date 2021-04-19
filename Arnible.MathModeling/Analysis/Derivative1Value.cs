using System;

namespace Arnible.MathModeling.Analysis
{
  interface IDerivative1
  {
    Number First { get; }
  }

  public readonly struct Derivative1Value : 
    IDerivative1, 
    IEquatable<Derivative1Value>, 
    IValueObject
  {
    public static readonly Derivative1Value Zero = new Derivative1Value(first: 0);

    public Derivative1Value(in Number first)
    {
      First = first;
    }
    
    public static explicit operator Derivative1Value(in Number v) => new Derivative1Value(v);

    public override bool Equals(object? obj)
    {
      if (obj is Derivative1Value typed)
      {
        return Equals(in typed);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(in Derivative1Value other)
    {
      return First == other.First;
    }

    public bool Equals(Derivative1Value other) => Equals(in other);

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public override string ToString()
    {
      return $"[{First.ToString()}]";
    }
    public string ToStringValue() => ToString();

    public static bool operator ==(in Derivative1Value a, in Derivative1Value b) => a.Equals(in b);
    public static bool operator !=(in Derivative1Value a, in Derivative1Value b) => !a.Equals(in b);

    //
    // Properties
    //

    public Number First { get; }
  }
}
