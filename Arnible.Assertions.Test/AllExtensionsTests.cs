using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class AllExtensionsTests
    {
        [Fact]
        public void IEnumerable_All_Error()
        {
            IEnumerable<int> toCheck = new int[] {1, 2};
            Assert.Throws<AssertException>(() => toCheck.AssertAll(i => i != 2));
        }
        
        [Fact]
        public void IEnumerable_All()
        {
            IEnumerable<int> toCheck = new int[] {1, 2};
            toCheck.AssertAll(i => i > 0);
        }
        
        [Fact]
        public void Span_All_Error()
        {
            Span<int> toCheck = new int[] {1, 2};
            try
            {
                toCheck.AssertAll((in int i) => i != 2);
                throw new Exception("I should not get here");
            }
            catch(AssertException)
            {
                // all is OK
            }
        }
        
        [Fact]
        public void Span_All()
        {
            Span<int> toCheck = new int[] {1, 2};
            toCheck.AssertAll((in int i) => i > 0);
        }
    }
}