using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IArray<T> : IEnumerable<T>
  {
    uint Length { get; }

    T this[uint index] { get; }
  }
}
