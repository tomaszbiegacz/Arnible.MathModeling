using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IFinitaryOperation<TNumber> where TNumber: struct
  {
    TNumber Value(in IEnumerable<TNumber> x);
  }  
}
