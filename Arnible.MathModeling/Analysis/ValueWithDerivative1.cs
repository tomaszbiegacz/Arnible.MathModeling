namespace Arnible.MathModeling.Analysis
{
  public readonly struct ValueWithDerivative1
  {
    public static implicit operator Derivative1Value(in ValueWithDerivative1 v) => new Derivative1Value
    {
      First = v.First
    };

    //
    // Properties
    //

    public Number Value { get; init; }

    public Number First { get; init; }
    
    //
    // Object
    //

    public override string ToString()
    {
      return $"{Value.ToString()} [{First.ToString()}]";
    }
  }
}