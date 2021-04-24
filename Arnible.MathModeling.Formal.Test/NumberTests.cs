using Arnible.Assertions;
using Arnible.MathModeling.Algebra.Polynomials;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    private static readonly Number A = Term.a;

    [Fact]
    public void Equality()
    {
      (0 == A - A).AssertIsTrue();
      (0 != A - A).AssertIsFalse();

      (1 == A + 1 - A).AssertIsTrue();
      (1 != A + 1 - A).AssertIsFalse();

      (1 == A - A).AssertIsFalse();
      (1 != A - A).AssertIsTrue();      
    }

    [Fact]
    public void Greater()
    {
      (0 > A + 1 - A).AssertIsFalse();
      (0 < A + 1 - A).AssertIsTrue();

      (2 > A + 1 - A).AssertIsTrue();
      (2 < A + 1 - A).AssertIsFalse();

      (2 * A > A).AssertIsFalse();
      (2 * A < A).AssertIsFalse();
      (2 * A >= A).AssertIsFalse();
      (2 * A <= A).AssertIsFalse();

      (A > Term.a).AssertIsFalse();
      (A < Term.a).AssertIsFalse();
      (A <= Term.a).AssertIsTrue();
      (A >= Term.a).AssertIsTrue();
    }
  }
}
