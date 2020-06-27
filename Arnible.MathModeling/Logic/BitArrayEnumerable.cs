using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling.Logic
{
  public class BitArrayEnumerable : IBitArray, IReadOnlyList<bool>
  {
    private readonly BitArray _bits;

    public BitArrayEnumerable(uint size)
    {
      _bits = new BitArray((int)size, false);
    }

    public BitArrayEnumerable(params bool[] flags)
    {
      _bits = new BitArray(flags);
    }

    private BitArrayEnumerable(IEnumerable<bool> bits)
    {
      _bits = new BitArray(bits.ToArray());
    }

    private static IEnumerable<bool> EnumerableFromNumber(uint value)
    {
      while (value > 0)
      {
        yield return value % 2 > 0;
        value /= 2;
      }
    }

    public static BitArrayEnumerable FromNumber(uint value)
    {      
      return new BitArrayEnumerable(EnumerableFromNumber(value));
    }

    public override string ToString()
    {
      return $"[{string.Join(',', this)}]";
    }

    /*
     * IBitArray
     */

    public bool this[uint index] => _bits[(int)index];

    public uint Length => (uint)_bits.Length;

    /*
     * IReadOnlyList
     */

    bool IReadOnlyList<bool>.this[int index] => _bits[index];

    int IReadOnlyCollection<bool>.Count => _bits.Length;

    public IEnumerator<bool> GetEnumerator()
    {
      foreach (bool v in _bits)
      {
        yield return v;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => _bits.GetEnumerator();

    /*
     * Operations
     */

    private bool AddBit(int pos)
    {
      if(pos < Length)
      {
        if(_bits[pos])
        {
          if(AddBit(pos + 1))
          {
            _bits[pos] = false;
            return true;
          }
        }
        else
        {
          _bits[pos] = true;
          return true;
        }
      }
      
      return false;      
    }

    public bool MoveNext()
    {
      return AddBit(0);
    }
  }
}
