public class TraverseXML
{
    private static Tree tree = new Tree();
    public static List<Tree> results = new List<Tree>();

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
    public static void Run(IEnumerable<XElement> elements, CompareScript? node)
    {
        foreach (var element in elements)
        {
            tree = new Tree();
            if (Recursive(element, node, tree))
            {
                results.Add(tree);
            }
        }
    }
    private static bool Recursive(XElement element, CompareScript? node, Tree branch)
    {
        //First Check if element is the one on CompareScript
        if (element.Name != node?.Tag) return false;
        branch.Tag = node.Tag;

        if (node.Filter?.Count() > 0 && element.HasAttributes)
        {
            var namesOfFilter = node.Filter.Select(x => x.Name);
            var attr = element.Attributes().Where(
                attributeItem =>
                node.Filter.SingleOrDefault(
                    x => x.Name == attributeItem.Name)?.Value == attributeItem.Value
                );
            if (attr is null || attr?.Count() == 0) return false;
            if (attr?.Count() != node.Filter.Count()) return false;
            branch.Filter.AddRange(node.Filter);
        }

        if (node.Report?.Count() > 0 && element.HasElements)
        {
            var children = element.Elements().Where(
                elementName =>
                   node.Report.Any(name => name == elementName.Name)
            );

            if (children?.Count() > 0)
            {
                foreach (var child in children)
                {
                    branch.Report.Add(child.Name.ToString(), child.Value);
                }
            }
        }

        if (node.Compare?.Count() > 0 && element.HasElements)
        {
            var children = element.Elements().Where(
               elementName =>
                  node.Compare.Any(name => name == elementName.Name)
           );

            if (children?.Count() > 0)
            {
                foreach (var child in children)
                {
                    branch.Compare.Add(child.Name.ToString(), child.Value);
                }
            }
        }

        if (node.Node is not null && element.HasElements)
        {
            var child = element.Element(node.Node.Tag);
            if (child is not null)
            {
                branch.Node = new Tree();
                return Recursive(child, node.Node, branch.Node);
            }
        }

        if (node.Compare is null || node.Compare?.Count() == 0) return false;

        return true;
    }

}