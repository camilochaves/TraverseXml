namespace TraverseXml.Models;

public class TreeStructure
{
    public string Tag { get; set; }
    public List<AttributeX>? Filter {get; set;} 
    public List<string>? Report {get; set;} 
    public List<string>? Compare {get; set;}
    public TreeStructure? Node {get; set;}

}