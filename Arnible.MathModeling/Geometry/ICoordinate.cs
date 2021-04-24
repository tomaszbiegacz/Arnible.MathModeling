namespace Arnible.MathModeling.Geometry
{
  interface ICoordinate<T> where T: struct
  {
    ushort DimensionsCount { get; }

    T AddDimension();
  }
}
