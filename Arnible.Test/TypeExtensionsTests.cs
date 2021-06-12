using System;
using Xunit;

namespace Arnible.Test
{
  public class TypeExtensionsTests
  {
    class EverybodyEqual<T> : IEquatable<EverybodyEqual<T>>
    {
      public bool Equals(EverybodyEqual<T>? other) => true;
      
      public override bool Equals(object? obj) => true;
      
      public override int GetHashCode() => 0;
    }
    
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
      Assert.True(typeof(EverybodyEqual<>).IsImplementingGenericInterface(typeof(IEquatable<>)));
    }
    
    [Fact]
    public void IsImplementingGenericInterface_NotInterface()
    {
      try
      {
        typeof(EverybodyEqual<>).IsImplementingGenericInterface(typeof(Exception));
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    } 
  }
}