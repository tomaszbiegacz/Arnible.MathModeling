using System.Collections.Generic;

namespace Arnible.MathModeling
{
  interface IValueArray<T> : IEnumerable<T>
    where T : struct
  {
    uint Length { get; }

    ref readonly T this[uint index] { get; }
  }
}
