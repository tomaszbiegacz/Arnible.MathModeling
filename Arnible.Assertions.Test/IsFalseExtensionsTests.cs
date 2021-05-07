using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsFalseExtensionsTests
    {
        [Fact]
        public void True()
        {
            Assert.Throws<AssertException>(() => true.AssertIsFalse());
        }
        
        [Fact]
        public void False()
        {
            false.AssertIsFalse();
        }
    }
}