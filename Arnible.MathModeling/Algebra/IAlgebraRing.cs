namespace Arnible.MathModeling.Algebra
{
  public interface IAlgebraRing<T> : IAlgebraGroup<T>
  {
    T Multiply(in T factor);
  }
}