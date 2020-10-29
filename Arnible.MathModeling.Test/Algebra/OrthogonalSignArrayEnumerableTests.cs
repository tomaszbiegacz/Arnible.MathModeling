using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public class OrthogonalSignArrayEnumerableTests : UnmanagedArrayEnumerableTests<Sign>
  {    
    [Fact]
    public void Collection_1()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(1).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, Sign.None);
      VerifyAndFinish(items, Sign.Positive);      
    }
    
    [Fact]
    public void Collection_1_1()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(1, 1).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndFinish(items, Sign.Positive);      
    }

    [Fact]
    public void Collection_2()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(2).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, Sign.None, Sign.None);

      VerifyAndMove(items, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Negative, Sign.Positive);
      VerifyAndFinish(items, Sign.Positive, Sign.Positive);
    }
    
    [Fact]
    public void Collection_2_1()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(2, 1).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, Sign.Positive, Sign.None);
      VerifyAndFinish(items, Sign.None, Sign.Positive);
    }
    
    [Fact]
    public void Collection_2_2()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(2, 2).GetEnumerator();
      IsTrue(items.MoveNext());
      
      VerifyAndMove(items, Sign.Negative, Sign.Positive);
      VerifyAndFinish(items, Sign.Positive, Sign.Positive);
    }

    [Fact]
    public void Collection_3()
    {
      using var items = SignArrayEnumerable.GetOrthogonalSignCollection(3).GetEnumerator();
      IsTrue(items.MoveNext());
      VerifyAndMove(items, Sign.None, Sign.None, Sign.None);

      VerifyAndMove(items, Sign.Positive, Sign.None, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Negative, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.Positive, Sign.Positive, Sign.None);
      
      VerifyAndMove(items, Sign.None, Sign.Negative, Sign.Positive);
      VerifyAndMove(items, Sign.Negative, Sign.None, Sign.Positive);
      VerifyAndMove(items, Sign.Positive, Sign.None, Sign.Positive);
      VerifyAndMove(items, Sign.None, Sign.Positive, Sign.Positive);

      VerifyAndMove(items, Sign.Negative, Sign.Negative, Sign.Positive);
      VerifyAndMove(items, Sign.Positive, Sign.Negative, Sign.Positive);
      VerifyAndMove(items, Sign.Negative, Sign.Positive, Sign.Positive);
      VerifyAndFinish(items, Sign.Positive, Sign.Positive, Sign.Positive);
    }
  }
}
