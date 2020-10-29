using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public class BitArrayEnumerableTests : UnmanagedArrayEnumerableTests<bool>
  {
    [Fact]
    public void Collection_1()
    {
      using var items = new BoolArrayEnumerable(1).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, false);
      VerifyAndFinish(items, true);
    }

    [Fact]
    public void Collection_2()
    {
      using var items = new BoolArrayEnumerable(2).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, false, false);

      VerifyAndMove(items, true, false);
      VerifyAndMove(items, false, true);

      VerifyAndFinish(items, true, true);
    }

    [Fact]
    public void Collection_3()
    {
      using var items = new BoolArrayEnumerable(3).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, false, false, false);

      VerifyAndMove(items, true, false, false);
      VerifyAndMove(items, false, true, false);
      VerifyAndMove(items, false, false, true);

      VerifyAndMove(items, true, true, false);
      VerifyAndMove(items, true, false, true);
      VerifyAndMove(items, false, true, true);      

      VerifyAndFinish(items, true, true, true);
    }    
  }
}
