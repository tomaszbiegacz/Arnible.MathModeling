namespace Arnible.MathModeling.Geometry
{
  interface ICoordinate<T> where T: struct
  {
    uint DimensionsCount { get; }

    T AddDimension();
  }
}
