using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqArrayTests
  {
    [Fact]
    public void Indexes()
    {
      Assert.Equal(new[] { 0u, 1u, 2u }, (new[] { 1, 2, 3 }).Indexes());
    }

    [Fact]
    public void ToJaggedArray()
    {
      Assert.Equal(new int[][] { new[] { 0, 1, 2 }, new[] { 1, 2, 3 } }, new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJagged());
    }

    [Fact]
    public void ToInversedJaggedArray()
    {
      Assert.Equal(new int[][] { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 2, 3 } }, new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJaggedInversed());
    }
  }
}
