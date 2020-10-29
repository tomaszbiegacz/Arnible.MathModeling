using System.Collections.Generic;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public abstract class UnmanagedArrayEnumerableTests<TValue>
    where TValue : unmanaged
  {
    protected static void Verify(UnmanagedArray<TValue> list, params TValue[] signs)
    {
      AreEqual(signs.Length, list.Length);
      for (uint i = 0; i < signs.Length; ++i)
      {
        // ReSharper disable once HeapView.PossibleBoxingAllocation
        // ReSharper disable once HeapView.BoxingAllocation
        IsTrue(signs[i].Equals(list[i]));
      }
    }

    protected static void VerifyAndMove(IEnumerator<UnmanagedArray<TValue>> list, params TValue[] signs)
    {
      Verify(list.Current, signs);
      IsTrue(list.MoveNext());
    }

    protected static void VerifyAndFinish(IEnumerator<UnmanagedArray<TValue>> list, params TValue[] signs)
    {
      Verify(list.Current, signs);
      IsFalse(list.MoveNext());
    }
  }
}
