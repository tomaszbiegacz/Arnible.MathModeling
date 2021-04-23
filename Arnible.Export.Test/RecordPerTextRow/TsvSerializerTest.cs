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
      var record = new TestRecord
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
        var serializer = TestRecordWriterBuilder.Default.CreateTsvValueRecordWriter<TestRecord>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split('\n');
      lines.Length.AssertIsEqualTo(2);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue", "NumberArray_0", "NumberArray_1"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t1.3\t1.4");
    }
    
    [Fact]
    public void InlineSerialization()
    {
      var record = new ValueRecord
      {
        RootValue = 1,
        Record = new TestSubRecord(2),
        Nullable = null,
        OtherValue = 3
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = TestRecordWriterBuilder.Default.CreateTsvValueRecordWriter<ValueRecord>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }

      string[] lines = result.Split('\n');
      lines.Length.AssertIsEqualTo(2);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[] { "RootValue", "Record_Value", "Nullable_NotPresentValue", "Nullable_NotPresentOther", "OtherValue" });
      lines[1].AssertIsEqualTo("1\t2\t\t\t3");
    }
    
    [Fact]
    public void ArraySerialization()
    {
      var record = new TestRecordArray
      {
        Records = new[] {
          new NullableSubRecord(),
          new NullableSubRecord
          {
            NotPresentValue = 1,
            NotPresentOther = 5
          }
        },
        RecordsValuesNotNull = new TestRecord[0]
      };

      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var serializer = TestRecordWriterBuilder.Default.CreateTsvValueRecordWriter<TestRecordArray>(writer);

        serializer.Write(in record);
        writer.Flush(out result);
      }
      
      string[] lines = result.Split('\n');
      lines.Length.AssertIsEqualTo(2);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther" });
      lines[1].AssertIsEqualTo("2\t3\t1\t5");
    }
  }
}
