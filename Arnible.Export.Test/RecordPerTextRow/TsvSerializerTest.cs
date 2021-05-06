using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Arnible.Assertions;
using Arnible.Export.Test;
using Arnible.Logger;
using Arnible.MathModeling;
using Xunit;

namespace Arnible.Export.RecordPerTextRow.Test
{
  public class TsvSerializerTest
  {
    [Fact]
    public void DefaultSerialization()
    {
      var record = new TestValueRecord
      {
        ByteValue = 1,
        SbyteValue = -1,
        UShortValue = 2,
        ShortValue = -2,
        UIntValue = 3,
        IntValue = -3,
        ULongValue = 4,
        LongValue = -4,
        StringValue = "value",
        DoubleValue = 1.1,
        NumberValue = 1.2,
        NumberArray = new Number[] { 1.3, 1.4 }
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvValueRecordWriter<TestValueRecord>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue", "NumberArray_0", "NumberArray_1"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t1.3\t1.4");
      lines[2].AssertIsEmpty();
    }
    
    [Fact]
    public void DefaultSerialization_NullReadOnlyArray()
    {
      var record = new TestValueRecord
      {
        ByteValue = 1,
        SbyteValue = -1,
        UShortValue = 2,
        ShortValue = -2,
        UIntValue = 3,
        IntValue = -3,
        ULongValue = 4,
        LongValue = -4,
        StringValue = "value",
        DoubleValue = 1.1,
        NumberValue = 1.2
      };
      
      TestValueRecord? record2 = null;
      TestValueRecord? record3 = record;

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvValueRecordWriter<TestValueRecord>(writer);

        serializer.Write(in record);
        serializer.Write(in record2);
        serializer.Write(in record3);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(5);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2");
      lines[2].AssertIsEqualTo("\t\t\t\t\t\t\t\t\t\t");
      lines[3].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2");
      lines[4].AssertIsEmpty();
    }
    
    [Fact]
    public void InlineSerialization()
    {
      var record = new ValueRecord
      {
        RootValue = 1,
        Record = new TestSubReferenceRecord(2),
        Nullable = null,
        OtherValue = 3
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvValueRecordWriter<ValueRecord>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[] { "RootValue", "Record_Value", "Nullable_NotPresentValue", "Nullable_NotPresentOther", "OtherValue" });
      lines[1].AssertIsEqualTo("1\t2\t\t\t3");
      lines[2].AssertIsEmpty();
    }
    
    [Fact]
    public void ArraySerialization()
    {
      var record = new TestReferenceRecordArray
      {
        Records = new[] {
          new NullableSubRecord(),
          new NullableSubRecord
          {
            NotPresentValue = 1,
            NotPresentOther = 5
          }
        },
        RecordsValuesNotNull = new TestValueRecord[0],
        RecordsReferencesList = new List<NullableSubRecord>
        {
          new NullableSubRecord()
        },
        RecordsValuesList = new List<TestSubValueRecord>
        {
          new TestSubValueRecord(10)
        }
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvValueRecordWriter<TestReferenceRecordArray>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "Records_0_NotPresentValue", "Records_0_NotPresentOther", 
        "Records_1_NotPresentValue", "Records_1_NotPresentOther",
        "RecordsReferencesList_0_NotPresentValue", "RecordsReferencesList_0_NotPresentOther",
        "RecordsValuesList_0_Value"
      });
      lines[1].AssertIsEqualTo("2\t3\t1\t5\t2\t3\t10");
      lines[2].AssertIsEmpty();
    }
    
    [Fact]
    public void RefValueSerializationTest()
    {
      var record = new TestRefValueRecordArray
      {
        Pos = 'a',
        Value = 1.2f,
        Price = 1.3m,
        Gross = 1.4m,
        Description = "first",
        Values = stackalloc[] { 1, 2 },
        References = stackalloc TestSubValueRecord[]
        {
          new TestSubValueRecord(3),
          new TestSubValueRecord(4)
        }
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvRecordWriter(writer);
        
        using (var recordSerializer = serializer.OpenRecord())
        {
          record.Serialize(recordSerializer.FieldSerializer);
        }
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "Pos", "Value", "Price", "Gross", "Description",
        "Values_0", "Values_1",
        "References_0_Value", "References_1_Value"
      });
      lines[1].AssertIsEqualTo("a\t1.2\t1.3\t1.4\tfirst\t1\t2\t3\t4");
      lines[2].AssertIsEmpty();
    }
    
    [Fact]
    public void ArraysRecordTest()
    {
      var record = new TestArraysReferenceRecord
      {
        ByteValue = new byte[] { 1 },
        SbyteValue = new sbyte[] { -1 },
        UShortValue = new ushort[] { 2 },
        ShortValue = new short[] { -2 },
        UIntValue = new uint[] { 3 },
        ULongValue = new ulong[] { 4 },
        LongValue = new long[] { -4 },
        FloatValue = new float[] { 1.1f },
        DoubleValue = new double[] { 1.2 },
        DecimalValue = new decimal[] { 1.3m }
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvReferenceRecordWriter<TestArraysReferenceRecord>(writer);

        serializer.Write(record);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue_0", "SbyteValue_0", "UShortValue_0", "ShortValue_0", "UIntValue_0", "ULongValue_0", "LongValue_0", 
        "FloatValue_0", "DoubleValue_0", "DecimalValue_0"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t4\t-4\t1.1\t1.2\t1.3");
      lines[2].AssertIsEmpty();
    }
    
    [Fact]
    public void NullValueRecord()
    {
      TestSubValueRecord? record = null;

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = RecordWriterBuilderForTests.Default.CreateTsvValueRecordWriter<TestSubValueRecord>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split(Environment.NewLine);
      lines.Length.AssertIsEqualTo(3);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "Value"
      });
      lines[1].AssertIsEqualTo(""); // empty value
      lines[2].AssertIsEmpty();
    }
  }
}
