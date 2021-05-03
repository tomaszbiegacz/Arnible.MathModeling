using Xunit;

namespace Arnible.Linq.Test
{
  public class AnyTests
  {
    [Fact]
    public void Boolean_OK()
    {
      Assert.True(new bool[] { false, true}.Any());
    }
    
    [Fact]
    public void Boolean_Missing()
    {
      Assert.False(new bool[] { false, false}.Any());
    }
  }
}