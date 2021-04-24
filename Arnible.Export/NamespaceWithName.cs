using System;

namespace Arnible.Export
{
  public class NamespaceWithName
  {
    private readonly NamespaceWithName? _prefix;
    private string? _prefixFullName;
    private string _name;
    private string _fullName;

    public NamespaceWithName(string separator)
    {
      _prefix = null;
      _prefixFullName = null;
      NameSeparator = separator;
      
      _name = string.Empty;
      _fullName = string.Empty;
    }
    
    private NamespaceWithName(NamespaceWithName prefix, string separator, string name)
    {
      if(string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException(nameof(name));
      }
      _prefix = prefix;
      _prefixFullName = _prefix.FullName;
      NameSeparator = separator;
      
      _name = name;
      _fullName = GetFullName();
    }
    
    public string NameSeparator { get; }

    public string FullName
    {
      get
      {
        if(_prefix != null && !object.ReferenceEquals(_prefixFullName, _prefix.FullName))
        {
          _prefixFullName = _prefix.FullName;
          _fullName = GetFullName();
        }
        return _fullName;
      }
    }

    private string GetFullName()
    {
      if(string.IsNullOrEmpty(_prefixFullName))
      {
        return _name;
      }
      else
      {
        return string.Concat(_prefixFullName, NameSeparator, _name);
      }
    }

    public void SetName(string name)
    {
      if(string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException(nameof(name));
      }
      
      _prefixFullName = _prefix?.FullName ?? string.Empty;
      _name = name;
      _fullName = GetFullName();
    }
    
    public NamespaceWithName SubName(string name)
    {
      if(name.Length == 0)
        return this;
      else
        return new NamespaceWithName(this, separator: NameSeparator, name: name);
    }
  }
}