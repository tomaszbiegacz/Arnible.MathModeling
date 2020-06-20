using System;

namespace Arnible.MathModeling
{
  interface IDerivative2 : IDerivative1
  {
    Number Second { get; }
  }

  public readonly struct Derivative2Value : IDerivative2, IEquatable<Derivative2Value>
  {
    public Derivative2Value(Number first, Number second)
    {
      First = first;
      Second = second;
    }

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

    public bool Equals(Derivative2Value other)
    {
      return First == other.First && Second == other.Second;
    }

    public override int GetHashCode()
    {
      return First.GetHashCode() ^ Second.GetHashCode();
    }

    public override string ToString()
    {
      return $"[{First}, {Second}]";
    }

    public static bool operator ==(Derivative2Value a, Derivative2Value b) => a.Equals(b);
    public static bool operator !=(Derivative2Value a, Derivative2Value b) => !a.Equals(b);

    public static implicit operator Derivative1Value(Derivative2Value v) => new Derivative1Value(v.First);

    //
    // Properties
    //

    public Number First { get; }

    public Number Second { get; }    
  }
}
