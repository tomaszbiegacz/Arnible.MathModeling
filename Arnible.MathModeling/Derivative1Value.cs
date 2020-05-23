namespace Arnible.MathModeling
{
  public readonly struct Derivative1Value : IDerivative1
  {
    public static readonly Derivative1Value Zero = new Derivative1Value(0);

    public Derivative1Value(Number first)
    {
      First = first;
    }

    public Number First { get; }

    public override bool Equals(object obj)
    {
      if(obj is IDerivative1 typed)
      {
        return Equals(typed);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(IDerivative1 other)
    {
      return First == other?.First;
    }

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }

    public override string ToString()
    {
      return $"[{First}]";
    }
  }
}
