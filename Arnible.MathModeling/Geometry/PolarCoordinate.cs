using System;

namespace Arnible.MathModeling.Geometry
{
  public struct PolarCoordinate
  {
    public Number R { get; }

    public Number Φ { get; }

    public PolarCoordinate(Number r, Number φ)
    {
      R = r;
      Φ = φ;

      if (r < 0)
      {
        throw new ArgumentException($"Negative r: {r}");
      }

      if (r == 0)
      {
        if (φ != 0)
        {
          throw new ArgumentException($"For zero r, angle also has to be empty, got {φ}");
        }
      }
      else
      {
        if (φ < 0)
        {
          throw new ArgumentException($"Found negative angular cooridnate: {φ}");
        }
        if (φ >= 2 * Math.PI)
        {
          throw new ArgumentException($"Invalid angualr coordinate: {φ}");
        }
      }
    }
  }
}
