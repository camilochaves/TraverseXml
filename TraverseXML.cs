namespace TraverseXml;

public static class TraverseXML
{
    private static TreeStructureReport tree = new();
    private static List<TreeStructureReport> results = new();

    public static void Run(IEnumerable<XElement>? elements, TreeStructure? configNode, TreeStructureReport? branch = null)
    {
        foreach (var element in elements)
        {
            if (element.Parent?.Parent is null)
            {
                tree = new TreeStructureReport();
                branch = tree;
            }
            if (!ElementNameMatchTag(element, configNode))
            {
                continue;
            }

            //Filters are optional but if they exist and don´t match
            //then this is not your element to process 
            if (!ProcessFilters(element, configNode, branch))
            {
                continue;
            }

            branch.Tag = configNode.Tag;
            //Reports are optional
            ProcessReports(element, configNode, branch);
            //Compares are optional
            ProcessCompares(element, configNode, branch);

            if (element.HasElements && configNode.Node is not null)
            {
                branch.Node = new TreeStructureReport();
                Run(element.Elements(), configNode.Node, branch.Node);
            }
            else
            {
                if (tree is not null)
                {
                    results.Add(tree.Clone());
                    tree = tree.RemoveBranch(branch);
                    branch = new TreeStructureReport();
                    if(tree is not null) tree.Node = branch;
                }
            }
        }
    }

    private static bool ElementNameMatchTag(XElement element, TreeStructure? configNode) =>
        (element.Name.ToString() == configNode?.Tag);

    private static bool ProcessFilters(XElement element, TreeStructure? configNode, TreeStructureReport branch)
    {
        //Are there any filters to consider ?
        if (configNode.Filter?.Count() > 0 && element.HasAttributes)
        {
            var attr = element.Attributes().Where(
                attributeItem =>
                    configNode.Filter.SingleOrDefault(
                        x => x.Name == attributeItem.Name)?.Value == attributeItem.Value
            );
            //If you have N filters, then you must have N attributes on this element
            if (!attr.Any() || attr.Count() != configNode.Filter.Count())
            {
                //If you don't then this is not the element you are looking for
                //Return and continue to the next element
                return false;
            }

            branch.Filter.AddRange(configNode.Filter);
        }
        return true;
    }

    private static void ProcessReports(XElement element, TreeStructure? configNode, TreeStructureReport branch)
    {
        //Do you have any Fields to be included in the report ?
        if (configNode.Report?.Count() > 0 && element.HasElements)
        {
            IEnumerable<XElement> childrenReport = element.Elements().Where(
                elementName =>
                    configNode.Report.Any(name => name == elementName.Name)
            );

            if (childrenReport.Any())
            {
                foreach (var child in childrenReport)
                {
                    Tuple<string, string> valueToAdd = new(child.Name.ToString(), child.Value);
                    branch.Report.Add(valueToAdd);
                    //if (!branch.Report.Exists(x => x.Equals(valueToAdd))) branch.Report.Add(valueToAdd);
                }
            }
        }
    }
    private static void ProcessCompares(XElement element, TreeStructure? configNode, TreeStructureReport branch)
    {
        //Do you have any Fields to compare ?
        if (configNode.Compare?.Count() > 0 && element.HasElements)
        {
            var children = element.Elements().Where(
                elementName =>
                    configNode.Compare.Any(name => name == elementName.Name)
            );

            if (children.Any())
            {
                foreach (var child in children)
                {
                    Tuple<string, string> valueToAdd = new(child.Name.ToString(), child.Value);
                    //You cannot add duplicated Tuples
                    if (!branch.Compare.Exists(x => x.Equals(valueToAdd))) branch.Compare.Add(valueToAdd);
                }
            }
        }
    }

    public static List<string> Report(
     string export = "unique",
     string output = "console",
     List<string>? seed = null)
    {

        var tempCompareList = new List<List<string>>();

        if (export == "equal" && seed is not null)
        {
            tempCompareList.Add(seed);
        }

        var report = new List<string>();
        foreach (var result in results)
        {
            var compareList = result.CompareList().ToList();
            if (tempCompareList.Count == 0)
            {
                tempCompareList.Add(compareList);
                report.Add(result.ToString());
            }
            else
            {
                if (export == "unique")
                {
                    var isContained = tempCompareList.Any(x => x.SequenceEqual(compareList));
                    if (!isContained)
                    {
                        tempCompareList.Add(compareList);
                        report.Add(result.ToString());
                    }
                }
                else if (export == "equal")
                {
                    var isContained = tempCompareList.Any(x => x.SequenceEqual(compareList));
                    if (isContained)
                    {
                        tempCompareList.Add(compareList);
                        report.Add(result.ToString());
                    }
                }
                else report.Add(result.ToString());
            }
        }
        return report;
    }
}