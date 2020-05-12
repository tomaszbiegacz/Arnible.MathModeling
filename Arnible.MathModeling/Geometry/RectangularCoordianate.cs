namespace Arnible.MathModeling.Geometry
{
  public interface IRectangularCoordianate
  {
    Number X { get; }
    Number Y { get; }
  }

  public readonly struct RectangularCoordianate : IRectangularCoordianate
  {
    public Number X { get; }

    public Number Y { get; }

    public RectangularCoordianate(Number x, Number y)
    {
      X = x;
      Y = y;
    }
  }
}
