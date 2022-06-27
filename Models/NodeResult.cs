namespace Model;

public class Tree
{
    public string Tag { get; set; }
    public List<AttributeX> Filter { get; set; } = new List<AttributeX>();
    public Dictionary<string, string> Report { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> Compare { get; set; } = new Dictionary<string, string>();
    public Tree? Node { get; set; }

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
            result = result + $" Filters:[{filterString}] ";
        }
        if (Report.Count > 0)
        {
            foreach (var report in Report)
            {
                reportString = reportString + $"{report.Key}:{report.Value},";
            }
            result = result + $" Reports:[{reportString}] ";
        }
        if (Compare.Count > 0)
        {
            foreach (var compare in Compare)
            {
                compareString = compareString + $"{compare.Key}:{compare.Value},";
            }
            result = result + $" Compares:[{compareString}]";
        }
        if (Node is not null)
        {
            result = result + $" SubNode: {Node.Tag}-> {Node.ToString()}";
        }
        return result;
    }

    public IEnumerable<string> CompareList()
    {
        if(Node is null) return Compare.Values.AsEnumerable<string>();
        IEnumerable<string> currentNodeList = Compare.Values.AsEnumerable<string>();
        return currentNodeList.Concat(Node.CompareList());
    }
}