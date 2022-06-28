namespace TraverseXml.Models;

public class AttributeX
{
    public string Name { get; set; }
    public string Value { get; set; }
    public override string ToString()
    {
        return $"{Name}:{Value}";
    }
}