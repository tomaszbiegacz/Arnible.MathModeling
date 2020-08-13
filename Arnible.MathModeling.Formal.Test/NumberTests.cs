using Arnible.MathModeling.Polynomials;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    private static readonly Number A = Term.a;

    [Fact]
    public void Equality()
    {
      IsTrue(0 == A - A);
      IsFalse(0 != A - A);

      IsTrue(1 == A + 1 - A);
      IsFalse(1 != A + 1 - A);

      IsFalse(1 == A - A);
      IsTrue(1 != A - A);      
    }

    [Fact]
    public void Greater()
    {
      IsFalse(0 > A + 1 - A);
      IsTrue(0 < A + 1 - A);

      IsTrue(2 > A + 1 - A);
      IsFalse(2 < A + 1 - A);

      IsFalse(2 * A > A);
      IsFalse(2 * A < A);
      IsFalse(2 * A >= A);
      IsFalse(2 * A <= A);

      IsFalse(A > Term.a);
      IsFalse(A < Term.a);
      IsTrue(A <= Term.a);
      IsTrue(A >= Term.a);
    }
  }
}
