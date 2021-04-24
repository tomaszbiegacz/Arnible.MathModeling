namespace Arnible.MathModeling.Algebra
{
  public interface IAlgebraUnitRing<T> : IAlgebraRing<T>
  {
    ref readonly T One { get; }
  }
}