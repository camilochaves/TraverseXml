using System.Collections.Generic;

namespace TraverseXml.Models;

public class TreeConfig
{
    public string XML { get; set; }

    //valid values: unique, all, equal
    public string Export {get; set;}
    public List<string>? EqualTo {get; set;}

    //valid values: console, excel
    public string Output {get; set;}
    public TreeStructure Config {get; set;}

}