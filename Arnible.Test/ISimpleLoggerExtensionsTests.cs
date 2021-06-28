using System;
using Arnible.Logger;
using Xunit;

namespace Arnible.Test
{
    public class ISimpleLoggerExtensionsTests
    {
        private readonly SimpleLoggerMemoryWriter _writer = new SimpleLoggerMemoryWriter();

        [Fact]
        public void Write_2()
        {
            _writer.Write("s1", "s2");
            _writer.Flush(out string result);
            Assert.Equal("s1s2", result);
        }
        
        [Fact]
        public void Write_3()
        {
            _writer.Write("s1", "s2", "s3");
            _writer.Flush(out string result);
            Assert.Equal("s1s2s3", result);
        }
        
        [Fact]
        public void Write_4()
        {
            _writer.Write("s1", "s2", "s3", "s4");
            _writer.Flush(out string result);
            Assert.Equal("s1s2s3s4", result);
        }
        
        [Fact]
        public void Write_5()
        {
            _writer.Write("s1", "s2", "s3", "s4", "s5");
            _writer.Flush(out string result);
            Assert.Equal("s1s2s3s4s5", result);
        }
        
        [Fact]
        public void Write_6()
        {
            _writer.Write("s1", "s2", "s3", "s4", "s5", "s6");
            _writer.Flush(out string result);
            Assert.Equal("s1s2s3s4s5s6", result);
        }
        
        [Fact]
        public void Write_7()
        {
            _writer.Write("s1", "s2", "s3", "s4", "s5", "s6", "s7");
            _writer.Flush(out string result);
            Assert.Equal("s1s2s3s4s5s6s7", result);
        }
    }
}