namespace TraverseXml.Models;

public class TreeStructureReport
{
    public string Tag { get; set; }
    public List<AttributeX> Filter { get; set; } = new();
    public List<Tuple<string, string>> Report { get; set; } = new();
    public List<Tuple<string, string>> Compare { get; set; } = new();
    public TreeStructureReport? Node { get; set; }

    public override string ToString()
    {
        string filterString = ""; string reportString = ""; string compareString = "";
        string result = $"Tag:{Tag}";
        if (Filter.Count > 0)
        {
            foreach (var filter in Filter)
            {
                filterString = filterString + filter.ToString() + " ";
            }
            result += $" Filters:[{filterString}] ";
        }
        if (Report.Any())
        {
            foreach (var report in Report)
            {
                reportString += $"{report.Item1}:{report.Item2},";
            }
            result += $" Reports:[{reportString}] ";
        }
        if (Compare.Any())
        {
            foreach (var compare in Compare)
            {
                compareString += $"{compare.Item1}:{compare.Item2},";
            }
            result += $" Compares:[{compareString}]";
        }
        if (Node is not null)
        {
            result += $" SubNode: {Node.Tag}-> {Node}";
        }
        return result;
    }

    public IEnumerable<string> CompareList()
    {
        if (Node is null) return Compare.Select(x => x.Item2).AsEnumerable<string>();
        IEnumerable<string> currentNodeList = Compare.Select(x => x.Item2).AsEnumerable<string>();
        return currentNodeList.Concat(Node.CompareList());
    }

    public void AddBranch(TreeStructureReport branch)
    {
        Node = branch;
    }

    public TreeStructureReport? RemoveBranch(TreeStructureReport branch)
    {
        if (Tag == branch.Tag) return null;
        if (Node is null) return null;
        Node = Node.RemoveBranch(branch);        
        return this;
    }

    public TreeStructureReport Clone()
    {
        return new TreeStructureReport
        {
            Tag = this.Tag,
            Filter = Filter,
            Report = Report,
            Compare = Compare,
            Node = Node?.Clone()
        };
    }
}