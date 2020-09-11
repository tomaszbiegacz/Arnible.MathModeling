using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  public interface IRecordWriterReadOnlyCollection<TRecord>
  {
    void Write(in IReadOnlyCollection<TRecord>? record);
  }
}