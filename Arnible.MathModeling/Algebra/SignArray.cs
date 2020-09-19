using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  readonly struct SignArray : IComparable<SignArray>
  {
    public SignArray(IEnumerable<sbyte> sings)
    {
      Values = sings.ToReadOnlyList();
    }

    /*
     * IComparable
     */

    public int CompareTo(SignArray other)
    {
      int byLength = Values.Count.CompareTo(other.Values.Count);
      if (byLength != 0)
      {
        return byLength;
      }
      
      uint valueCount = Values.Where(s => s != 0).Count();
      uint valueCountOther = other.Values.Where(s => s != 0).Count();
      int byValueCount = valueCount.CompareTo(valueCountOther);
      if (byValueCount != 0)
      {
        return byValueCount;
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

    public IReadOnlyList<sbyte> Values { get; }
    
    /*
     * Operations
     */
    
    public bool GetIsOrthogonal()
    {
      uint negativeCount = Values.Where(s => s < 0).Count();
      if (negativeCount == 0)
      {
        // only positive direction
        return true;
      }

      sbyte lastNonZero = Values.Where(s => s != 0).Last();
      return lastNonZero > 0;
    }
  }
}
