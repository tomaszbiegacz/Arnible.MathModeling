using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  readonly struct BoolArray : IComparable<BoolArray>
  {
    private readonly uint _valueCount;

    public BoolArray(IEnumerable<bool> values)
    {
      Values = values.ToReadOnlyList();
      _valueCount = Values.Where(s => s).Count();
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

      for (int iPos = 0; iPos < Values.Count; iPos++)
      {
        int i = Values.Count - 1 - iPos;
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

    public IReadOnlyList<bool> Values { get; }
  }
}
