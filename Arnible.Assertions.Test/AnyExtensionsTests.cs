using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class AnyExtensionsTests
    {
        [Fact]
        public void IEnumerable_Any_Error()
        {
            IEnumerable<int> toCheck = new int[] {1, 2};
            Assert.Throws<AssertException>(() => toCheck.AssertAny(i => i == 3));
        }
        
        [Fact]
        public void IEnumerable_Any()
        {
            IEnumerable<int> toCheck = new int[] {1, 2};
            toCheck.AssertAny(i => i == 2);
        }
        
        [Fact]
        public void Span_Any_Error()
        {
            Span<int> toCheck = new int[] {1, 2};
            try
            {
                toCheck.AssertAny((in int i) => i == 3);
                throw new Exception("I should not get here");
            }
            catch(AssertException)
            {
                // All is OK
            }
        }
        
        [Fact]
        public void Span_Any()
        {
            Span<int> toCheck = new int[] {1, 2};
            toCheck.AssertAny((in int i) => i == 2);
        }
    }
}