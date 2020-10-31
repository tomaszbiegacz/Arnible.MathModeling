namespace Arnible.MathModeling.Optimization
{
  public readonly struct ValueWithDerivative1 : IValueObject
  {
    public ValueWithDerivative1(Number x, Number y, Number first)
    {
      X = x;
      Y = y;
      First = first;
    }
    
    public Number X { get; }
    
    public Number Y { get; }
    
    public Number First { get; }

    public override int GetHashCode()
    {
      return X.GetHashCode() ^ Y.GetHashCode() ^ First.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public string ToStringValue()
    {
      return $"({X.ToStringValue()}, {Y.ToStringValue()}, {First.ToStringValue()})";
    }
  }
}