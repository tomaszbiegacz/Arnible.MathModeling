using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsBetweenExtensionsTest
    {
        [Fact]
        public void AssertIsBetween_Error()
        {
            Assert.Throws<AssertException>(() => 3.AssertIsBetween(1, 2));
        }
        
        [Fact]
        public void AssertIsBetween_Ok()
        {
            3.AssertIsBetween(1, 4);
        }
    }
}