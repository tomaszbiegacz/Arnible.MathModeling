namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowCollectionItemFieldSerializer : RecordPerRowFieldSerializer
  {
    private ushort _position;
    
    public RecordPerRowCollectionItemFieldSerializer(RecordPerRowFieldSerializer parent)
    : base(parent, "0")
    {
      _position = 0;
    }
    
    public ushort Position
    {
      get => _position;
      set
      {
        _position = value;
        _fieldNamespace.SetName(_position.ToString());
      }
    }
  }
}