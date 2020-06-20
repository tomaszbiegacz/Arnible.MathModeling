using System;

namespace Arnible.MathModeling
{
  interface IDerivative1
  {
    Number First { get; }
  }

  public readonly struct Derivative1Value : IDerivative1, IEquatable<Derivative1Value>
  {
    public static readonly Derivative1Value Zero = new Derivative1Value(0);

    public Derivative1Value(Number first)
    {
      First = first;
    }    

    public override bool Equals(object obj)
    {
      if (obj is IDerivative1 typed)
      {
        return Equals(typed);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(Derivative1Value other)
    {
      return First == other.First;
    }

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }

    public override string ToString()
    {
      return $"[{First}]";
    }

    public static bool operator ==(Derivative1Value a, Derivative1Value b) => a.Equals(b);
    public static bool operator !=(Derivative1Value a, Derivative1Value b) => !a.Equals(b);

    //
    // Properties
    //

    public Number First { get; }    
  }
}
