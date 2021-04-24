using System;

namespace Arnible.MathModeling.Analysis
{
  public interface IFinitaryOperation<TNumber> where TNumber: struct
  {
    TNumber Value(in ReadOnlySpan<TNumber> x);
  }  
}
