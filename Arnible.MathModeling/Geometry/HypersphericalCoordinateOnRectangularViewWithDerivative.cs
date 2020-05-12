using System;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct HypersphericalCoordinateOnRectangularViewWithDerivative :
    IEquatable<HypersphericalCoordinateOnRectangularViewWithDerivative>,
    IEquatable<HypersphericalCoordinateOnRectangularView>,
    IHypersphericalCoordinateOnRectangularView
  {
    private readonly HypersphericalCoordinateOnRectangularView _view;

    public HypersphericalCoordinateOnRectangularViewWithDerivative(
      HypersphericalCoordinateOnRectangularView view,
      IDerivative1 xDerivative,
      IDerivative1 yDerivative)
    {
      _view = view;
      RatioXDerivative = xDerivative ?? throw new ArgumentNullException(nameof(xDerivative));
      RatioYDerivative = yDerivative ?? throw new ArgumentNullException(nameof(yDerivative));
    }

    public static implicit operator HypersphericalCoordinateOnRectangularView(HypersphericalCoordinateOnRectangularViewWithDerivative v) => v._view;

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalCoordinateOnRectangularViewWithDerivative casted)
      {
        return Equals(casted);
      }
      else if (obj is HypersphericalCoordinateOnRectangularView casted2)
      {
        return Equals(casted2);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(HypersphericalCoordinateOnRectangularViewWithDerivative other)
    {
      return _view.Equals(other._view) && RatioXDerivative.Equals(other.RatioXDerivative) && RatioYDerivative.Equals(other.RatioYDerivative);
    }

    public bool Equals(HypersphericalCoordinateOnRectangularView other)
    {
      return _view.Equals(other);
    }

    public override int GetHashCode()
    {
      int hash = 17;
      hash = hash * 23 + _view.GetHashCode();
      hash = hash * 23 + RatioXDerivative.GetHashCode();
      hash = hash * 23 + RatioYDerivative.GetHashCode();
      return hash;
    }

    public override string ToString()
    {
      return _view.ToString();
    }

    //
    // Properties
    //

    public IDerivative1 RatioXDerivative { get; }

    public IDerivative1 RatioYDerivative { get; }

    //
    // IHypersphericalCoordinateOnRectangularView
    //

    public Number R => _view.R;

    public Number RatioX => _view.RatioX;

    public Number RatioY => _view.RatioY;

    public Number X => _view.X;

    public Number Y => _view.Y;
  }
}
