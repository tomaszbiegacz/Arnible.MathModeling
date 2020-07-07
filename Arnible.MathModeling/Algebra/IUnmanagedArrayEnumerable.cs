namespace Arnible.MathModeling.Algebra
{
  public interface IUnmanagedArrayEnumerable<T> : IUnmanagedArray<T>
    where T : unmanaged
  {
    bool MoveNext();
  }
}
