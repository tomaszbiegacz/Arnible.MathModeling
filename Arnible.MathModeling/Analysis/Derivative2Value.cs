namespace Arnible.MathModeling.Analysis
{
  interface IDerivative2 : IDerivative1
  {
    Number Second { get; }
  }

  public readonly struct Derivative2Value : IDerivative2
  {
    public Derivative2Value(in Number first, in Number second)
    {
      First = first;
      Second = second;
    }

    public static implicit operator Derivative1Value(in Derivative2Value v) => new Derivative1Value(v.First);

    //
    // Properties
    //

    public Number First { get; }

    public Number Second { get; }    
    
    //
    // Object
    //
    
    public override string ToString()
    {
      return $"{First.ToString()}, {Second.ToString()}";
    }
  }
}
