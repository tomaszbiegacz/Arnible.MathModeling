using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.Logger.Test
{
  public class SimpleLoggerMemoryWriterTests
  {
    [Fact]
    public async Task Flush_FileStream()
    {
      string tempFilePath = Path.GetTempFileName();

      await using(Stream stream = File.OpenWrite(tempFilePath))
      using(SimpleLoggerMemoryWriter writer = new())
      {
        writer.Write("First-line").NewLine();
        writer.Write("Second-line");
        
        await writer.Flush(stream, default);
      }
      
      string content = await File.ReadAllTextAsync(tempFilePath, default);
      Assert.Equal($"First-line{Environment.NewLine}Second-line", content);
    }
    
    [Fact]
    public async Task Flush_MemoryStream()
    {
      await using (MemoryStream stream = new MemoryStream())
      {
        using(SimpleLoggerMemoryWriter writer = new())
        {
          writer.Write("First-line").NewLine();
          writer.Write("Second-line");
        
          await writer.Flush(stream, default);
        }
        
        string content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal($"First-line{Environment.NewLine}Second-line", content);
      }
    }
    
    [Fact]
    public void Flush_OtherLogger()
    {
      using (SimpleLoggerMemoryWriter other = new())
      {
        using(SimpleLoggerMemoryWriter writer = new())
        {
          writer.Write("First-line").NewLine();
          writer.Write("Second-line");
        
          writer.Flush(other);
        }

        other.Flush(out string content);
        Assert.Equal($"First-line{Environment.NewLine}Second-line", content);  
      }
    }
  }
}