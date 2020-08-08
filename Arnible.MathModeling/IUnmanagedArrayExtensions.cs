using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class IUnmanagedArrayExtensions
  {
    public static IUnmanagedArray<T> SubsetFromIndexes<T>(
      this IUnmanagedArray<T> src,
      in IReadOnlyCollection<uint> indexes
    ) where T : unmanaged
    {
      T[] result = new T[indexes.Count];

      uint i = 0;
      foreach (uint index in indexes)
      {
        result[i] = src[index];
        i++;
      }

      return new UnmanagedArray<T>(result);
    }
  }
}