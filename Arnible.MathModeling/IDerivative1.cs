using System;

namespace Arnible.MathModeling
{
  public interface IDerivative1 : IEquatable<IDerivative1>
  {
    Number First { get; }    
  }
}
