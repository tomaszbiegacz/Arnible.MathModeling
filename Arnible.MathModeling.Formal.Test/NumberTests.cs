using Arnible.Assertions;
using Arnible.Logger;
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
      
      (A + 1 - A).Equals(1).AssertIsTrue();
      
      (A + 1).GetHashCode().AssertIsEqualTo((1 + A).GetHashCode());
    }
    
    [Fact]
    public void DoubleCast()
    {
      double val = (double)(A + 1 - A);
      val.AssertIsEqualTo(1);
    }
    
    [Fact]
    public void EqualsOtherObject()
    {
      Number val = 1;
      val.Equals(new int[0]).AssertIsFalse();
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
    
    [Fact]
    public void OneTest()
    {
      default(Number).One.AssertIsEqualTo(1);
    }
    
    [Fact]
    public void ZeroTest()
    {
      default(Number).Zero.AssertIsEqualTo(0);
    }
    
    [Fact]
    public void InverseTest()
    {
      A.Inverse().AssertIsEqualTo(-1 * A);
    }
    
    [Fact]
    public void WriteTest()
    {
      using var logger = new SimpleLoggerMemoryWriter();
      (2*A).Write(logger);
      logger.Flush(out string value);
      value.AssertIsEqualTo("2a");
      (2*A).ToString().AssertIsEqualTo("2a");
    }
    
    [Theory]
    [InlineData(0, 1, -1)]
    [InlineData(1, 1, 0)]
    [InlineData(2, 1, 1)]
    public void CompareTo(double first, double second, int result)
    {
      Number f = first;
      f.CompareTo(second).AssertIsEqualTo(result);
    }
    
    [Fact]
    public void NumberNumberTests()
    {
      Number other = 2;
      Number value = A;
      (value / other).AssertIsEqualTo(0.5 * A);
      (value + other).AssertIsEqualTo(A + 2);
      (value - other).AssertIsEqualTo(A - 2);
      (value * other).AssertIsEqualTo(2 * A);
      other.Multiply(value).AssertIsEqualTo(2*A);
    }
    
    [Fact]
    public void NumberPolynomialTests()
    {
      Polynomial other = 2;
      Number value = A;
      (value / other).AssertIsEqualTo(0.5 * A);
      (value + other).AssertIsEqualTo(A + 2);
      (value - other).AssertIsEqualTo(A - 2);
      (value * other).AssertIsEqualTo(2 * A);
    }
    
    [Fact]
    public void NumberDoubleTests()
    {
      double other = 2;
      Number value = A;
      (value / other).AssertIsEqualTo(0.5 * A);
      (value + other).AssertIsEqualTo(A + 2);
      (value - other).AssertIsEqualTo(A - 2);
      (value * other).AssertIsEqualTo(2 * A);
    }
    
    [Fact]
    public void NumberIntTests()
    {
      int other = 2;
      Number value = A;
      (value / other).AssertIsEqualTo(0.5 * A);
      (value + other).AssertIsEqualTo(A + 2);
      (value - other).AssertIsEqualTo(A - 2);
      (value * other).AssertIsEqualTo(2 * A);
    }
    
    [Fact]
    public void NumberUintTests()
    {
      uint other = 2;
      Number value = A;
      (value / other).AssertIsEqualTo(0.5 * A);
      (value + other).AssertIsEqualTo(A + 2);
      (value - other).AssertIsEqualTo(A - 2);
      (value * other).AssertIsEqualTo(2 * A);
    }
    
    [Fact]
    public void PolynomialNumberTests()
    {
      Polynomial other = Term.a;
      Number value = 2;
      (other / value).AssertIsEqualTo(0.5 * A);
      (other + value).AssertIsEqualTo(A + 2);
      (other - value).AssertIsEqualTo(A - 2);
      (other * value).AssertIsEqualTo(2 * A);
    }
    
    [Fact]
    public void DoubleNumberTests()
    {
      double other = 1;
      Number value = 2;
      (other / value).AssertIsEqualTo(0.5);
      (other + value).AssertIsEqualTo(3);
      (other - value).AssertIsEqualTo(-1);
      (other * value).AssertIsEqualTo(2);
    }
    
    [Fact]
    public void IntNumberTests()
    {
      int other = 1;
      Number value = 2;
      (other / value).AssertIsEqualTo(0.5);
      (other + value).AssertIsEqualTo(3);
      (other - value).AssertIsEqualTo(-1);
      (other * value).AssertIsEqualTo(2);
    }
    
    [Fact]
    public void UintNumberTests()
    {
      uint other = 1;
      Number value = 2;
      (other / value).AssertIsEqualTo(0.5);
      (other + value).AssertIsEqualTo(3);
      (other - value).AssertIsEqualTo(-1);
      (other * value).AssertIsEqualTo(2);
    }
  }
}
