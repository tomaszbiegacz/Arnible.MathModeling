using System;
using System.IO;
using System.Threading.Tasks;
using Arnible.Assertions;
using Arnible.Export.Test;
using Arnible.Logger;
using Arnible.MathModeling;
using Xunit;

namespace Arnible.Export.RecordPerTextRow.Test
{
  public class CreateTsvNotepadTests
  {
    [Fact]
    public async Task CreateTsvValueNotepadTests()
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

      string filePath;
      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var logger = writer.WithWriterFactory(TestRecordWriterBuilder.Default);
        await using (var serializer = logger.CreateTsvValueNotepad<TestValueRecord>("test"))
        {
          serializer.Write(in record);
          filePath = serializer.Destination.FullName;
        }
        
        writer.Flush(out result);
      }

      string[] lines =  await File.ReadAllLinesAsync(filePath);
      lines.Length.AssertIsEqualTo(2);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue", "NumberArray_0", "NumberArray_1"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t1.3\t1.4");
      
      string[] resultLines = result.Split(Environment.NewLine);
      resultLines.Length.AssertIsEqualTo(2);
      resultLines[0].AssertIsEqualTo("Notepad test: " + filePath);
    }
    
    [Fact]
    public async Task CreateTsvReferenceNotepadTests()
    {
      var record = new TestReferenceRecord
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
        NumberArray = new ReadOnlyArrayWrapper<Number>
        {
          Value = new Number[] { 1.3, 1.4 } 
        }
      };

      string filePath;
      string result;
      using (var writer = new SimpleLoggerMemoryWriter())
      {
        var logger = writer.WithWriterFactory(TestRecordWriterBuilder.Default);
        await using (var serializer = logger.CreateTsvReferenceNotepad<TestReferenceRecord>("test"))
        {
          serializer.Write(record);
          filePath = serializer.Destination.FullName;
        }
        
        writer.Flush(out result);
      }

      string[] lines =  await File.ReadAllLinesAsync(filePath);
      lines.Length.AssertIsEqualTo(2);
      lines[0].Split('\t').AssertSequenceEqualsTo(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue", "NumberArray_0", "NumberArray_1"
      });
      lines[1].AssertIsEqualTo("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t1.3\t1.4");
      
      string[] resultLines = result.Split(Environment.NewLine);
      resultLines.Length.AssertIsEqualTo(2);
      resultLines[0].AssertIsEqualTo("Notepad test: " + filePath);
    }
  }
}