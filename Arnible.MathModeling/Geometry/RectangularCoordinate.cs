namespace Arnible.MathModeling.Geometry
{
  public readonly struct RectangularCoordinate
  {
    public Number X { get; }

    public Number Y { get; }

    public RectangularCoordinate(in Number x, in Number y)
    {
      X = x;
      Y = y;
    }
  }
}
