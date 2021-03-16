namespace Arnible.Linq
{
  public static class LinqArray<T>
  {
    static readonly T[] _empty = new T[0];
    
    public static T[] Empty => _empty;
  }
}