namespace Arnible.MathModeling.Analysis.Optimization
{
  public readonly struct NumberFunctionPointWithDerivative
  {
    public NumberFunctionPointWithDerivative(in Number x, in Number y, in Number first)
    {
      X = x;
      Y = y;
      First = first;
    }
    
    public Number X { get; }
    
    public Number Y { get; }
    
    public Number First { get; }

    public void Write(ISimpleLogger logger)
    {
      logger.Write("{", X);
      logger.Write(", ", Y);
      logger.Write(") ", First);
    }
  }
}