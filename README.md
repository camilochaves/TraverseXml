# TraverseXml
## Traverse Xml Structure and Compare elements  
  
XML files could have many elements in many levels, and each, could have attributes. TraverseXML filters and compare the tree elements from the XML file according to a json configuration file.  

**Json file structure:**
  
```
  {
    "XML": "test.xml", //**Required**
    "Export": "unique" //**Required**: other options: unique, equal 
    "EqualTo": [], //**Optional** unless Export above is equal instead of unique, then you must specify expected result values as an array of strings
    "Output": "console", //**Required**: current output is only console, but excel output is coming 
    "Config": { //**Required**: This section is based on the same repeating structure to match the xml tree levels
        "Tag": "FirstLevel", //**Required** Tag is the name of the xml element on this level
        "Filter": [{  //**Optional**: filters are an array of xml attributes of the current element
            "Name": "active",
            "Value": "true"
        }],
        "Report": [],  //**Optional**: Name of the fields that must be in the report , those are not compared
        "Compare": [], //is **Optional** on all nodes except the last one. Must contain the **Name** of the fields that ought to be compared
        "Node": {
            "Tag": "SecondLevel", //Xml element whose name is SecondLevel . (Check the file test.xml)
            "Filter": [],
            "Report": [],
            "Compare": ["Name"],
            "Node":{}
        }
    }
}
```
>  dotnet run config_SecondLevelComparison.json

```
Tag:FirstLevel Filters:[active:true]  SubNode: SecondLevel-> Tag:SecondLevel Compares:[Name:SonA]
Tag:FirstLevel Filters:[active:true]  SubNode: SecondLevel-> Tag:SecondLevel Compares:[Name:SonC]
Total records: 2
```
