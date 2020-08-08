using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  readonly struct SignArray : IComparable<SignArray>
  {
    private readonly uint _valueAbsCount;
    private readonly uint _valueNegativeCount;

    public SignArray(IEnumerable<sbyte> sings)
    {
      Values = sings.ToValueArray();
      _valueAbsCount = Values.Where(s => s != 0).Count();
      _valueNegativeCount = Values.Where(s => s < 0).Count();
    }

    /*
     * IComparable
     */

    public int CompareTo(SignArray other)
    {
      int byAbsCount = _valueAbsCount.CompareTo(other._valueAbsCount);
      if (byAbsCount != 0)
      {
        return byAbsCount;
      }

      int byNegativeCount = _valueNegativeCount.CompareTo(other._valueNegativeCount);
      if (byNegativeCount != 0)
      {
        return byNegativeCount;
      }

      int byLength = Values.Length.CompareTo(other.Values.Length);
      if (byLength != 0)
      {
        return byLength;
      }

      for (uint i = Values.Length - 1; i >= 0; --i)
      {
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

    public bool IsOrthogonal => _valueAbsCount == 0 || _valueAbsCount != _valueNegativeCount;

    public ValueArray<sbyte> Values { get; }
  }
}
