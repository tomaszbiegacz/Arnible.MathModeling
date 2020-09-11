using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public abstract class UnmanagedArrayEnumerableTests<TEnumerator, TValue>
    where TEnumerator : class, IUnmanagedArrayEnumerable<TValue>
    where TValue : unmanaged
  {
    protected static void Verify(TEnumerator list, params TValue[] signs)
    {
      AreEqual(signs.Length, list.Length);
      for (uint i = 0; i < signs.Length; ++i)
      {
        // ReSharper disable once HeapView.PossibleBoxingAllocation
        // ReSharper disable once HeapView.BoxingAllocation
        IsTrue(signs[i].Equals(list[i]));
      }
    }

    protected static void VerifyAndMove(TEnumerator list, params TValue[] signs)
    {
      Verify(list, signs);
      IsTrue(list.MoveNext());
    }

    protected static void VerifyAndFinish(TEnumerator list, params TValue[] signs)
    {
      Verify(list, signs);
      IsFalse(list.MoveNext());
      Verify(list, signs);
    }
  }
}
