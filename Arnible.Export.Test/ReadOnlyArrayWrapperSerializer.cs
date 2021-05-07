using System;

namespace Arnible.Export.Test
{
  public class ReadOnlyArrayWrapperSerializer<T> : IReferenceRecordSerializer<ReadOnlyArrayWrapper<T>> where T : IEquatable<T>
  {
    public void Serialize(IRecordFieldSerializer serializer, ReadOnlyArrayWrapper<T> record)
    {
      serializer.WriteValueField(string.Empty, record.Value);
    }
  }
}