namespace Arnible.MathModeling.Geometry
{
  public interface ICoordinate<T> where T: struct
  {
    uint DimensionsCount { get; }

    T AddDimension();
  }
}
