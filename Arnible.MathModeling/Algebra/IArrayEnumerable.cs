namespace Arnible.MathModeling.Algebra
{
  public interface IArrayEnumerable<T> : IArray<T> where T : struct
  {
    bool MoveNext();
  }
}
