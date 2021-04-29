namespace Arnible.MathModeling.Analysis.Optimization
{
  public readonly struct FunctionPointWithDerivative
  {
    public FunctionPointWithDerivative(in Number x, in Number y, in Number first)
    {
      X = x;
      Y = y;
      First = first;
    }
    
    public Number X { get; }
    
    public Number Y { get; }
    
    public Number First { get; }

    public string ToStringValue()
    {
      return $"({X.ToString()}, {Y.ToString()}) [{First.ToString()}]";
    }
  }
}