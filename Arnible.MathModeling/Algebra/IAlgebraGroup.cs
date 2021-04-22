using System;

namespace Arnible.MathModeling.Algebra
{
  public interface IAlgebraGroup<T> : IEquatable<T>
  {
    ref readonly T Zero { get; }
    
    T Add(in T component);
    
    T Inverse();
  }
}