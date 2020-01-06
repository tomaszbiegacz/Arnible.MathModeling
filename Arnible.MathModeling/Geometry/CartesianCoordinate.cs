using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public struct CartesianCoordinate : ICoordinate
  {
    private readonly Number[] _coordinates;

    public CartesianCoordinate(IEnumerable<Number> coordinates)
    {
      _coordinates = coordinates.ToArray();
    }

    public static implicit operator CartesianCoordinate(RectangularCoordianate rc)
    {
      return new CartesianCoordinate(new Number[] { rc.X, rc.Y });
    }

    public IEnumerable<Number> Coordinates => _coordinates ?? Enumerable.Empty<Number>();

    public uint DimensionsCount => (uint)Coordinates.Count();
  }
}
