using System;

namespace Arnible.MathModeling
{
  interface IDerivative2 : IDerivative1
  {
    Number Second { get; }
  }

  public readonly struct Derivative2Value : IDerivative2, IEquatable<Derivative2Value>
  {
    public Derivative2Value(in Number first, in Number second)
    {
      First = first;
      Second = second;
    }

    public static implicit operator Derivative1Value(in Derivative2Value v) => new Derivative1Value(v.First);

    public override bool Equals(object obj)
    {
      if (obj is Derivative2Value typed2)
      {
        return Equals(typed2);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(in Derivative2Value other)
    {
      return First == other.First && Second == other.Second;
    }

    public bool Equals(Derivative2Value other) => Equals(in other);

    public override int GetHashCode()
    {
      return First.GetHashCode() ^ Second.GetHashCode();
    }

    public override string ToString()
    {
      return $"[{First}, {Second}]";
    }

    public static bool operator ==(in Derivative2Value a, in Derivative2Value b) => a.Equals(in b);
    public static bool operator !=(in Derivative2Value a, in Derivative2Value b) => !a.Equals(in b);    

    //
    // Properties
    //

    public Number First { get; }

    public Number Second { get; }    
  }
}
