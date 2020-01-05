using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public struct HypersphericalCoordinate : ICoordinate
  {
    private readonly Number[] _angles;

    public Number R { get; }

    public HypersphericalCoordinate(Number r, params Number[] angles)
    {
      R = r;
      _angles = angles;

      if(r < 0)
      {
        throw new ArgumentException($"Negative r: {r}");
      }
      if(r == 0 && angles.Length > 0)
      {
        throw new ArgumentException($"For zero r, angles also has to be empty, got {angles}");
      }
      if(Angles.Any())
      {
        if(_angles.Any(a => a < 0))
        {
          throw new ArgumentException($"Found negative angular cooridnate: {_angles}");
        }
        if(_angles.Take(_angles.Length - 1).Any(a => a >= Math.PI))
        {
          throw new ArgumentException($"Invalid first angular coordinates: {angles}");
        }
        if(_angles.Last() >= 2*Math.PI)
        {
          throw new ArgumentException($"Invalid last angualr coordinate: {_angles.Last()}");
        }
      }
    }

    public static implicit operator HypersphericalCoordinate(PolarCoordinate pc) => new HypersphericalCoordinate(pc.R, pc.Φ);

    public IEnumerable<Number> Angles => _angles ?? Enumerable.Empty<Number>();

    public uint DimensionsCount => (uint)Angles.Count() + 1;
  }
}
