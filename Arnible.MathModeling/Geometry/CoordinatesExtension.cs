using static System.Math;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    public static PolarCoordinate ToPolar(this RectangularCoordianate p)
    {
      return new PolarCoordinate(
        r: Sqrt(p.X * p.X + p.Y * p.Y),
        φ: Atan2(p.Y, p.X));
    }
  }
}
