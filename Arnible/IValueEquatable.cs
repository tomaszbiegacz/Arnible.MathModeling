using System;

namespace Arnible
{
  public interface IValueEquatable<T> : IEquatable<T> where T: struct
  {
    bool Equals(in T other); 
  }
}