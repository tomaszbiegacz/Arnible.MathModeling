namespace Arnible.Export.Test
{
  public class TestArraysReferenceRecord
  {
    public byte[]? ByteValue { get; set; }

    public sbyte[]? SbyteValue { get; set; }

    public ushort[]? UShortValue { get; set; }

    public short[]? ShortValue { get; set; }

    public uint[]? UIntValue { get; set; }

    public ulong[]? ULongValue { get; set; }

    public long[]? LongValue { get; set; }

    public float[]? FloatValue { get; set; }
    
    public double[]? DoubleValue { get; set; }
    
    public decimal[]? DecimalValue { get; set; }
    
    //
    // Serializer
    //
    
    public class Serializer : IReferenceRecordSerializer<TestArraysReferenceRecord>
    {
      public void Serialize(IRecordFieldSerializer serializer, TestArraysReferenceRecord record)
      {
        serializer.CollectionField<byte>().Write(nameof(ByteValue), record.ByteValue);
        serializer.CollectionField<sbyte>().Write(nameof(SbyteValue), record.SbyteValue);
        serializer.CollectionField<ushort>().Write(nameof(UShortValue), record.UShortValue);
        serializer.CollectionField<short>().Write(nameof(ShortValue), record.ShortValue);
        serializer.CollectionField<uint>().Write(nameof(UIntValue), record.UIntValue);
        serializer.CollectionField<ulong>().Write(nameof(ULongValue), record.ULongValue);
        serializer.CollectionField<long>().Write(nameof(LongValue), record.LongValue);
        serializer.CollectionField<float>().Write(nameof(FloatValue), record.FloatValue);
        serializer.CollectionField<double>().Write(nameof(DoubleValue), record.DoubleValue);
        serializer.CollectionField<decimal>().Write(nameof(DecimalValue), record.DecimalValue);
      }
    }
  }
}