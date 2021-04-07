namespace Arnible.MathModeling.Analysis
{
  public readonly struct ValueWithDerivative1 : IDerivative1, IValueObject
  {
    public ValueWithDerivative1(in Number value, in Number first)
    {
      Value = value;
      First = first;
    }

    public static explicit operator Derivative1Value(in ValueWithDerivative1 v) => new Derivative1Value(v.First);

    public override string ToString()
    {
      return $"[{First.ToString()}]";
    }
    public string ToStringValue() => ToString();

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    //
    // Properties
    //

    public Number Value { get; }

    public Number First { get; }
  }
}