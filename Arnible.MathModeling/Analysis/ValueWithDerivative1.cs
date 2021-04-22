namespace Arnible.MathModeling.Analysis
{
  public readonly struct ValueWithDerivative1 : IDerivative1
  {
    public ValueWithDerivative1(in Number value, in Number first)
    {
      Value = value;
      First = first;
    }

    public static implicit operator Derivative1Value(in ValueWithDerivative1 v) => new Derivative1Value(v.First);

    //
    // Properties
    //

    public Number Value { get; }

    public Number First { get; }
    
    //
    // Object
    //

    public override string ToString()
    {
      return $"{Value.ToString()} [{First.ToString()}]";
    }
  }
}