using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Geometry
{
  public interface ICartesianCoordinate
  {
    NumberVector Coordinates { get; }
  }

  public readonly struct CartesianCoordinate : IEquatable<CartesianCoordinate>, ICartesianCoordinate, ICoordinate<CartesianCoordinate>
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

    public static implicit operator CartesianCoordinate(NumberArray rc)
    {
      return new CartesianCoordinate(rc);
    }

    public bool Equals(CartesianCoordinate other)
    {
      return other.Coordinates == Coordinates;
    }

    public override bool Equals(object obj)
    {
      if(obj is CartesianCoordinate typed)
      {
        return Equals(typed);
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

    public override string ToString()
    {
      return Coordinates.ToString();
    }

    //
    // Properties
    //

    public uint DimensionsCount => Coordinates.Length;

    //
    // Operations
    //

    public CartesianCoordinate AddDimension()
    {
      return new CartesianCoordinate(Coordinates.Append(0).ToVector());
    }    
  }
}
