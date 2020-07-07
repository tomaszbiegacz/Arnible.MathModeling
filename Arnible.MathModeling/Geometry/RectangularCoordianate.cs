namespace Arnible.MathModeling.Geometry
{
  interface IRectangularCoordianate
  {
    Number X { get; }
    Number Y { get; }
  }

  public readonly struct RectangularCoordianate : IRectangularCoordianate
  {
    public Number X { get; }

    public Number Y { get; }

    public RectangularCoordianate(in Number x, in Number y)
    {
      X = x;
      Y = y;
    }
  }
}
