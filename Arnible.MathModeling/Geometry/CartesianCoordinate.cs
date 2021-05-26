using System;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  interface ICartesianCoordinate
  {
    ReadOnlyArray<Number> Coordinates { get; }
  }

  public readonly struct CartesianCoordinate : 
    IEquatable<CartesianCoordinate>, 
    ICartesianCoordinate, 
    ICoordinate<CartesianCoordinate>
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

    public ReadOnlyArray<Number> Coordinates { get; }

    private CartesianCoordinate(in ReadOnlyArray<Number> coordinates)
    {
      Coordinates = coordinates;
    }

    public CartesianCoordinate(params Number[] args)
      : this((ReadOnlyArray<Number>)args)
    {
      // intentionally empty
    }
    
    public static implicit operator CartesianCoordinate(in RectangularCoordinate rc)
    {
      return new CartesianCoordinate(new[] { rc.X, rc.Y });
    }

    public static implicit operator CartesianCoordinate(Number[] rc)
    {
      return new CartesianCoordinate(rc);
    }
    
    public static implicit operator CartesianCoordinate(double[] rc)
    {
      return new CartesianCoordinate(rc.Select(v => (Number)v).ToArray());
    }
    
    public static implicit operator CartesianCoordinate(ReadOnlyArray<Number> rc)
    {
      return new CartesianCoordinate(rc);
    }
    
    public bool Equals(in CartesianCoordinate other)
    {
      return other.Coordinates == Coordinates;
    }

    public bool Equals(CartesianCoordinate other) => Equals(in other);

    public override bool Equals(object? obj)
    {
      if(obj is CartesianCoordinate typed)
      {
        return Equals(in typed);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return Coordinates.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public override string ToString()
    {
      return Coordinates.ToString();
    }
    public string ToStringValue() => ToString();

    //
    // Properties
    //

    public ushort DimensionsCount => Coordinates.Length;

    //
    // Operations
    //

    public CartesianCoordinate AddDimension()
    {
      var coordinates = Coordinates.AsList().Append(0).ToArray();
      return new CartesianCoordinate(coordinates);
    }    
  }
}
