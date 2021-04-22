namespace Arnible.MathModeling.Analysis
{
  interface IDerivative1
  {
    Number First { get; }
  }

  public readonly struct Derivative1Value : IDerivative1
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
    // Object
    //

    public override string ToString()
    {
      return First.ToString();
    }
  }
}
