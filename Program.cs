using FluentValidation;
using Validators;

try
{
    var config = "config.json";
    if (args.Count() > 0) config = args[0];

    using StreamReader streamReader = new StreamReader(config);
    string json = streamReader.ReadToEnd();
    var configScript = JsonConvert.DeserializeObject<TreeConfig>(json);
    if (configScript is null) throw new Exception(message: "Error: CompareScript cannot be null! Deserialize Error on CompareScript json file");

    var validator = new TreeConfigValidator();
    validator.ValidateAndThrow(configScript);

    XElement? root = XElement.Load(configScript!.XML!);
    if (!root.HasElements) throw new Exception("XML has no Elements!");

    TraverseXML.Run(root.Elements(), configScript.Config);
    var report = TraverseXML.Report(
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

