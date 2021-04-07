using Xunit;

namespace Arnible.Linq.Test
{
  public class LinqArrayTests
  {
    [Fact]
    public void EmptyTests()
    {
      Assert.Empty(LinqArray<uint>.Empty);
    }
    
    [Fact]
    public void ToJaggedArray()
    {
      var result = new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJagged();
      Assert.Equal(2, result.Length);
      Assert.True(result[0].SequenceEqual(new[] { 0, 1, 2 }));
      Assert.True(result[1].SequenceEqual(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void ToInvertedJaggedArray()
    {
      var result = new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJaggedInverted();
      Assert.Equal(3, result.Length);
      Assert.True(result[0].SequenceEqual(new[] { 0, 1 }));
      Assert.True(result[1].SequenceEqual(new[] { 1, 2 }));
      Assert.True(result[2].SequenceEqual(new[] { 2, 3 }));
    }
  }
}