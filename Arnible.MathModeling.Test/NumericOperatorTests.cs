using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void Zero_Zero_Equals()
    {
      Assert.True(NumericOperator.Equals(0d, 0d));
    }

    [Fact]
    public void Zero_Epsilon_Equals()
    {
      Assert.True(NumericOperator.Equals(0d, double.Epsilon));
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      Assert.True(NumericOperator.Equals(double.Epsilon, 0d));
    }

    [Fact]
    public void Zero_1E16_Equals()
    {
      Assert.True(NumericOperator.Equals(0d, 1E-16));
    }

    [Fact]
    public void Zero_2E16_NotEquals()
    {
      Assert.False(NumericOperator.Equals(0d, 2E-16));
    }

    [Fact]
    public void One_1E16_Equals()
    {
      Assert.True(NumericOperator.Equals(1d, 1.0000000000000001));
    }

    [Fact]
    public void One_2E16_NotEquals()
    {
      Assert.False(NumericOperator.Equals(1d, 1.0000000000000002));
    }
  }
}
