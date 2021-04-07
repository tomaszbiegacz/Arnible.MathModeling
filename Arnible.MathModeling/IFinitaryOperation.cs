using System;

namespace Arnible.MathModeling
{
  public interface IFinitaryOperation<TNumber> where TNumber: struct
  {
    TNumber Value(in ReadOnlySpan<TNumber> x);
  }  
}
