using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class SignArrayEnumerableTests : UnmanagedArrayEnumerableTests<SignArrayEnumerable, Sign>
  {
    // 1 -> 3
    
    [Fact]
    public void Collection_1()
    {
      var items = new SignArrayEnumerable(1);
      VerifyAndMove(items, Sign.None);
      VerifyAndMove(items, Sign.Negative);
      VerifyAndFinish(items, Sign.Positive);
    }

    // 2 -> 9
    
    [Fact]
    public void Collection_2()
    {
      var items = new SignArrayEnumerable(2);
      VerifyAndMove(items, Sign.None, Sign.None);
      
      VerifyAndMove(items, Sign.None, Sign.Negative);
      VerifyAndMove(items, Sign.Negative, Sign.None);
      VerifyAndMove(items, Sign.Positive, Sign.None);
      VerifyAndMove(items, Sign.None, Sign.Positive);

      VerifyAndMove(items, Sign.Negative, Sign.Negative);
      VerifyAndMove(items, Sign.Positive, Sign.Negative);
      VerifyAndMove(items, Sign.Negative, Sign.Positive);
      VerifyAndFinish(items, Sign.Positive, Sign.Positive);
    }
  }
}