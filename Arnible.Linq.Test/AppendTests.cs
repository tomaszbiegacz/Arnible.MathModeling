using Xunit;

namespace Arnible.Linq.Test
{
  public class AppendTests
  {
    [Fact]
    public void Append_Two()
    {
      Assert.True((new[] { 2d, 1d }).Append(3, 2).SequenceEqual(new[] { 2d, 1d, 3, 3 }));
    }
  }
}