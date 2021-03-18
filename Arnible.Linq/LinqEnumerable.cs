using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LinqEnumerable
  {
    public static IEnumerable<T> Yield<T>(T src)
    {
      yield return src;
    }

    public static IEnumerable<int> RangeInt(ushort length) => RangeInt(0, length);

    public static IEnumerable<int> RangeInt(int start, uint length)
    {
      for (int i = start; i < start + length; ++i)
      {
        yield return i;
      }
    }

    public static IEnumerable<uint> RangeUint(uint length) => RangeUint(0, length);

    public static IEnumerable<uint> RangeUint(uint start, uint length)
    {
      for (uint i = start; i < start + length; ++i)
      {
        yield return i;
      }
    }
    
    public static IEnumerable<ushort> RangeUshort(ushort length) => RangeUshort(0, length);

    public static IEnumerable<ushort> RangeUshort(ushort start, ushort length)
    {
      for (ushort i = start; i < start + length; ++i)
      {
        yield return i;
      }
    }

    public static IEnumerable<T> Repeat<T>(T item, uint length)
    {
      for (uint i = 0; i < length; ++i)
      {
        yield return item;
      }
    }
  }
}