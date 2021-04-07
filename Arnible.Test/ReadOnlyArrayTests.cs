using Xunit;

namespace Arnible.Test
{
  public class ReadOnlyArrayTests
  {
    [Fact]
    public void double_Basic()
    {
      ReadOnlyArray<double> src = default;
      Assert.Equal(0, src.Length);
      Assert.True(src.IsEmpty);
      Assert.True(src.Equals(default));
      
      ReadOnlyArray<double> other = new double[] { 1, 2 };
      Assert.Equal(2, other.Length);
      Assert.False(other.IsEmpty);
      Assert.False(other.Equals(src));
      Assert.NotEqual(other.GetHashCode(), src.GetHashCode());
      
      ReadOnlyArray<double> other2 = new double[] { 1, 1+1 };
      Assert.True(other.Equals(other2));
      Assert.Equal(other.GetHashCode(), other2.GetHashCode());
      
      ReadOnlyArray<double> other3 = new double[] { 1, 2, 3 };
      Assert.False(other.Equals(other3));
      Assert.NotEqual(other.GetHashCode(), other3.GetHashCode());
    }
  }
}