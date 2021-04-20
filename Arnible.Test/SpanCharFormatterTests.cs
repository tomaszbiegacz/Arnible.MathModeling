using System;
using Xunit;

namespace Arnible.Test
{
  public class SpanCharFormatterTests
  {
    [Fact]
    public void Int_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      int value = 34;
      Assert.Equal("34", SpanCharFormatter.ToString(value, in buffer).ToString());
    }
    
    [Fact]
    public void UInt_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      uint value = 34;
      Assert.Equal("34", SpanCharFormatter.ToString(value, in buffer).ToString());
    }
    
    [Fact]
    public void Long_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      long value = 34;
      Assert.Equal("34", SpanCharFormatter.ToString(in value, in buffer).ToString());
    }
    
    [Fact]
    public void ULong_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      ulong value = 34;
      Assert.Equal("34", SpanCharFormatter.ToString(in value, in buffer).ToString());
    }
    
    [Fact]
    public void Float_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      float value = 3.4f;
      Assert.Equal("3.4", SpanCharFormatter.ToString(value, in buffer).ToString());
    }
    
    [Fact]
    public void Double_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      double value = 3.4;
      Assert.Equal("3.4", SpanCharFormatter.ToString(in value, in buffer).ToString());
    }
    
    [Fact]
    public void Decimal_ToString()
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      decimal value = 3.4m;
      Assert.Equal("3.4", SpanCharFormatter.ToString(in value, in buffer).ToString());
    }
  }
}