using System.Collections.Generic;

namespace Arnible.MathModeling.Logic
{
  public interface IBitArray : IEnumerable<bool>
  {
    bool this[uint index] { get; }

    uint Length { get; }    
  }
}
