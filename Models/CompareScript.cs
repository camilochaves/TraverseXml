namespace Model;

public class CompareScript
{
    public string Tag { get; set; }
    public List<AttributeX>? Filter {get; set;} 
    public List<string>? Report {get; set;} 
    public List<string>? Compare {get; set;}
    public CompareScript? Node {get;set;}
    
}