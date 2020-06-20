using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  interface IRecordLabels
  {
    IEnumerable<string> Labels { get; }
  }
}
