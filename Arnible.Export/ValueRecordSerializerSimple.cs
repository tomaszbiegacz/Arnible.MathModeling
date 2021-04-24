namespace Arnible.Export
{
  public abstract class ValueRecordSerializerSimple<T> : IValueRecordSerializer<T> where T: struct 
  {
    public abstract void Serialize(IRecordFieldSerializer serializer, in T? record);
    
    public void Serialize(IRecordFieldSerializer serializer, in T record)
    {
      Serialize(serializer, new T?(record));
    }

    
  }
}