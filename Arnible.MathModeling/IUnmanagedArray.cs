using System.Collections.Generic;

namespace Arnible.MathModeling
{
  interface IUnmanagedArray<T> : IEnumerable<T>
    where T : unmanaged
  {
    uint Length { get; }

    T this[in uint index] { get; }
  }
}
