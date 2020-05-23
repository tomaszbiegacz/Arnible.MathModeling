using System;

namespace Arnible.MathModeling
{
  public interface IDerivative2 : IDerivative1, IEquatable<IDerivative2>
  {
    Number Second { get; }
  }
}
