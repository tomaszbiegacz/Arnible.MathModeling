using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsTrueExtensionsTests
    {
        [Fact]
        public void False()
        {
            Assert.Throws<AssertException>(() => false.AssertIsTrue());
        }
        
        [Fact]
        public void True()
        {
            true.AssertIsTrue();
        }
    }
}