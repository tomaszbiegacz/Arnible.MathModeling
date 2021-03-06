﻿using Arnible.MathModeling;

namespace Arnible.Export.Test
{
  public class TestReferenceRecord
  {
    public byte ByteValue { get; set; }

    public sbyte SbyteValue { get; set; }

    public ushort UShortValue { get; set; }

    public short ShortValue { get; set; }

    public uint UIntValue { get; set; }

    public int IntValue { get; set; }

    public ulong ULongValue { get; set; }

    public long LongValue { get; set; }

    public string? StringValue { get; set; }

    public double DoubleValue { get; set; }

    public Number NumberValue { get; set; }
    
    public ReadOnlyArrayWrapper<Number>? NumberArray { get; set; }
    
    //
    // Serialization
    //
    
    public class Serializer : IReferenceRecordSerializer<TestReferenceRecord>
    {
      public void Serialize(IRecordFieldSerializer serializer, TestReferenceRecord record)
      {
        serializer.Write(nameof(ByteValue), record.ByteValue);
        serializer.Write(nameof(SbyteValue), record.SbyteValue);
        serializer.Write(nameof(UShortValue), record.UShortValue);
        serializer.Write(nameof(ShortValue), record.ShortValue);
        serializer.Write(nameof(UIntValue), record.UIntValue);
        serializer.Write(nameof(IntValue), record.IntValue);
        serializer.Write(nameof(ULongValue), record.ULongValue);
        serializer.Write(nameof(LongValue), record.LongValue);
        serializer.Write(nameof(StringValue), record.StringValue);
        serializer.Write(nameof(DoubleValue), record.DoubleValue);
        serializer.WriteValueField(nameof(NumberValue), record.NumberValue);
        serializer.WriteReferenceField(nameof(NumberArray), record.NumberArray);
      }
    }
  }
}
