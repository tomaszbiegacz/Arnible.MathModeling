namespace Arnible.MathModeling
{
  public interface IDerivative
  {
    double First { get; }

    double Second { get; }
  }

  public abstract class Derivative
  {
    public abstract double First { get; }

    public abstract double Second { get; }
  }
}
