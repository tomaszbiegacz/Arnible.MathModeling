using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.Test
{
  public class TypeExtensionsTests
  {
    class ExampleException : Exception
    {
      public ExampleException(string message)
      : base(message)
      {
        // intentionally empty
      }
    }
    
    [Fact]
    public void HasParameterlessConstructor_True()
    {
      Assert.True(typeof(TypeExtensionsTests).HasParameterlessConstructor());
    }
    
    [Fact]
    public void HasParameterlessConstructor_False()
    {
      Assert.False(typeof(ExampleException).HasParameterlessConstructor());
    }
    
    [Fact]
    public void IsImplementingGenericInterface_False()
    {
      Assert.False(typeof(ExampleException).IsImplementingGenericInterface(typeof(IEquatable<>)));
    }
    
    [Fact]
    public void IsImplementingGenericInterface_True()
    {
      Assert.True(typeof(ReadOnlyArray<>).IsImplementingGenericInterface(typeof(IEquatable<>)));
    }
    
    [Fact]
    public void IsImplementingGenericInterface_NotInterface()
    {
      try
      {
        typeof(ReadOnlyArray<>).IsImplementingGenericInterface(typeof(Exception));
        throw new Exception("I should not get here");
      }
      catch(AssertException)
      {
        // all is ok
      }
    } 
  }
}