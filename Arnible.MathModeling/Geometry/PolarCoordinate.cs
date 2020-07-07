using System;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct PolarCoordinate
  {
    public Number R { get; }

    /// <summary>
    /// Angle from y axis to vector
    /// </summary>
    public Number Φ { get; }

    public PolarCoordinate(in Number r, in Number φ)
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
        if (φ < -1 * Angle.HalfCycle || φ >= Angle.HalfCycle)
        {
          throw new ArgumentException($"Invalid angualr coordinate: {φ}");
        }
      }
    }
  }
}
