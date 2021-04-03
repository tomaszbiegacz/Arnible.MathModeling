using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
{
  readonly struct BoolArray : IComparable<BoolArray>
  {
    private readonly uint _valueCount;

    public BoolArray(IEnumerable<bool> values)
    {
      Values = values.ToUnmanagedArray();
      // ReSharper disable once HeapView.BoxingAllocation
      _valueCount = Values.Count(s => s);
    }

    /*
     * IComparable
     */

    public int CompareTo(BoolArray other)
    {
      int byCount = _valueCount.CompareTo(other._valueCount);
      if (byCount != 0)
      {
        return byCount;
      }      

      for (uint iPos = 0; iPos < Values.Length; iPos++)
      {
        uint i = Values.Length - 1 - iPos;
        int byValue = Values[i].CompareTo(other.Values[i]);
        if (byValue != 0)
        {
          return byValue;
        }
      }

      return 0;
    }

    /*
     * Properties
     */

    public UnmanagedArray<bool> Values { get; }
  }
}
