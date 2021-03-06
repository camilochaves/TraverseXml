try
{
    var config = "config.json";
    if (args.Count() > 0) config = args[0];

    using StreamReader streamReader = new StreamReader(config);
    string json = streamReader.ReadToEnd();
    var configScript = JsonConvert.DeserializeObject<TreeConfig>(json);
    if (configScript is null) throw new Exception(message: "Error: TreeStructure cannot be null! Deserialize Error on TreeStructure json file");

    var validator = new TreeConfigValidator();
    validator.ValidateAndThrow(configScript);

    XElement? root = XElement.Load(configScript!.XML!);
    if (!root.HasElements) throw new Exception("XML has no Elements!");

    var topLevelIterator = new TraverseXML();
    topLevelIterator.Run(root.Elements(), configScript.Config);
    var report = topLevelIterator.Report(
        configScript.Export,
        configScript.Output,
        configScript.EqualTo);

    foreach (var item in report)
    {
        WriteLine(item);
    }
    WriteLine($"Total records: {report.Count}");

}
catch (Exception ex)
{
    WriteLine($"Ooops {ex.Message}");
}

