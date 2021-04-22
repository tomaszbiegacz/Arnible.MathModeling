using System;

namespace Arnible.MathModeling.Algebra
{
  public interface IAlgebraUnitRing<T> : IAlgebraGroup<T>
  {
    ref readonly T One { get; }
    
    T Multiply(in T factor);
  }
}