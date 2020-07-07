using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class OrthogonalSignArrayEnumerableTests : UnmanagedArrayEnumerableTests<OrthogonalSignArrayEnumerable, Sign>
  {    
    [Fact]
    public void Collection_1()
    {
      var items = new OrthogonalSignArrayEnumerable(1);
      VerifyAndMove(items, Sign.None);
      VerifyAndFinish(items, Sign.Positive);      
    }

    [Fact]
    public void Collection_2()
    {
      var items = new OrthogonalSignArrayEnumerable(2);
      VerifyAndMove(items, Sign.None, Sign.None);

      VerifyAndMove(items, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Negative);
      VerifyAndFinish(items, Sign.Negative, Sign.Positive);
    }

    [Fact]
    public void Collection_3()
    {
      var items = new OrthogonalSignArrayEnumerable(3);
      VerifyAndMove(items, Sign.None, Sign.None, Sign.None);

      VerifyAndMove(items, Sign.Positive, Sign.None, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.Positive, Sign.None, Sign.Positive);
      VerifyAndMove(items, Sign.None, Sign.Positive, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.None, Sign.Negative);
      VerifyAndMove(items, Sign.None, Sign.Positive, Sign.Negative);
      VerifyAndMove(items, Sign.Positive, Sign.Negative, Sign.None);
      VerifyAndMove(items, Sign.Negative, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Negative, Sign.Positive);
      VerifyAndMove(items, Sign.Negative, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Positive, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Positive, Sign.Negative);
      VerifyAndMove(items, Sign.Positive, Sign.Negative, Sign.Positive);
      VerifyAndMove(items, Sign.Negative, Sign.Positive, Sign.Positive);

      VerifyAndMove(items, Sign.Positive, Sign.Negative, Sign.Negative);
      VerifyAndMove(items, Sign.Negative, Sign.Positive, Sign.Negative);
      VerifyAndFinish(items, Sign.Negative, Sign.Negative, Sign.Positive);
    }
  }
}
