using System;

namespace Arnible.MathModeling
{
  public class Derivative2Lazy : IDerivative2
  {
    private readonly Lazy<Number> _second;

    public Derivative2Lazy(Number first, Func<Number> second)
    {
      First = first;
      _second = new Lazy<Number>(second);
    }

    public Number First { get; }

    public Number Second => _second.Value;

    public override bool Equals(object obj)
    {
      if (obj is IDerivative2 typed2)
      {
        return Equals(typed2);
      }
      else if (obj is IDerivative1 typed)
      {
        return Equals(typed);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(IDerivative2 other)
    {
      return First == other?.First && Second == other?.Second;
    }

    public bool Equals(IDerivative1 other)
    {
      return First == other?.First;
    }

    public override int GetHashCode()
    {
      return First.GetHashCode() ^ Second.GetHashCode();
    }

    public override string ToString()
    {
      return $"[{First}, {Second}]";
    }
  }
}
