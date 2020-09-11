using System.IO;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public class TsvStreamWriter
  {
    private readonly LoggerTextSerializer _writer;

    public TsvStreamWriter(Stream stream)
    {
      _writer = new LoggerTextSerializer(new StreamWriter(stream));
      
      FieldSerializer = new RecordPerRowSerializerField(
        _writer, 
        headerPartsSeparator: TsvConst.HeaderFieldSeparator,
        fieldSeparator: TsvConst.RowFieldSeparator);
    }

    public IRecordFieldSerializer FieldSerializer { get; }
    
    public Task Flush()
    {
      return _writer.Flush();
    }
  }
}