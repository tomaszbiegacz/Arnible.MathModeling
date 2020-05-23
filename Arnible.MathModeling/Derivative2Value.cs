namespace Arnible.MathModeling
{
  public readonly struct Derivative2Value : IDerivative2
  {
    public Derivative2Value(Number first, Number second)
    {
      First = first;
      Second = second;
    }

    public Number First { get; }

    public Number Second { get; }

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
