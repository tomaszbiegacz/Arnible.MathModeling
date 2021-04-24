using System;
using System.Collections.Generic;

namespace Arnible.Export
{
  public interface IRecordCollectionWriter<TRecord>
  {
    void Write(in ReadOnlySpan<TRecord> record);
    void Write(TRecord[]? record);
    void Write(IReadOnlyCollection<TRecord>? record);
  }
}