namespace Arnible.MathModeling.Analysis
{
  public readonly struct Derivative2Value
  {
    public static implicit operator Derivative1Value(in Derivative2Value v) => new Derivative1Value
    {
      First = v.First
    };

    //
    // Properties
    //

    public Number First { get; init; }

    public Number Second { get; init; }
    
    //
    // Object
    //
    
    public override string ToString()
    {
      return $"{First.ToString()}, {Second.ToString()}";
    }
  }
}
