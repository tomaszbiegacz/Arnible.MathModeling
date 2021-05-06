using System;

namespace Arnible.Export
{
  public interface IRecordSerializer : IDisposable
  {
    IRecordFieldSerializer FieldSerializer { get; }
  }
}