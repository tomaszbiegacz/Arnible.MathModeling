namespace Arnible.MathModeling.Geometry
{
  interface IRectangularCoordinate
  {
    Number X { get; }
    Number Y { get; }
  }

  public readonly struct RectangularCoordinate : IRectangularCoordinate
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
