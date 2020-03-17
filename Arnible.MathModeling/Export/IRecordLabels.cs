using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  public interface IRecordLabels
  {
    IEnumerable<string> Labels { get; }
  }
}
