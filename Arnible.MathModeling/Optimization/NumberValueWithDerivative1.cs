namespace Arnible.MathModeling.Optimization
{
  public readonly struct NumberValueWithDerivative1 : IValueObject
  {
    public NumberValueWithDerivative1(Number x, Number y, Number first)
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
      return $"(x: {X.ToStringValue()}, y: {Y.ToStringValue()}, d: {First.ToStringValue()})";
    }
  }
}