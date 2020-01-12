using Arnible.MathModeling.Algebra;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public struct CartesianCoordinate : ICoordinate<CartesianCoordinate>
  {
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
