using Arnible.MathModeling.Algebra;
using System;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct CartesianCoordinate : ICoordinate<CartesianCoordinate>
  {
    public static CartesianCoordinate ForAxis(uint dimensionsCount, uint axisNumber, Number value)
    {
      if (dimensionsCount == 0)
        throw new ArgumentException(nameof(dimensionsCount));
      if (axisNumber >= dimensionsCount)
        throw new ArgumentException(nameof(dimensionsCount));

      var coordinates = new Number[dimensionsCount];
      coordinates[axisNumber] = value;
      return new CartesianCoordinate(coordinates);
    }

    public NumberVector Coordinates { get; }

    public CartesianCoordinate(NumberVector coordinates)
    {
      Coordinates = coordinates;
    }

    public CartesianCoordinate(params Number[] args)
      : this(args.ToVector())
    {
      // intentionally empty
    }

    public static implicit operator CartesianCoordinate(RectangularCoordianate rc)
    {
      return new CartesianCoordinate(new Number[] { rc.X, rc.Y }.ToVector());
    }

    public override string ToString()
    {
      return Coordinates.ToString();
    }

    public uint DimensionsCount => (uint)Coordinates.Count;

    public CartesianCoordinate AddDimension()
    {
      return new CartesianCoordinate(Coordinates.Append(0).ToVector());
    }
  }
}
