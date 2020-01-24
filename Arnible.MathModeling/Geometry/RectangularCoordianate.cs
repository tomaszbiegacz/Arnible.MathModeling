﻿namespace Arnible.MathModeling.Geometry
{
  public readonly struct RectangularCoordianate
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
