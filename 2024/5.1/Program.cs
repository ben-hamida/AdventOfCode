var lines = File.ReadLines("input.txt").ToArray();

var rules = lines.TakeWhile(line => line != "").Select(Rule.Parse).ToArray();
var updates = lines.SkipWhile(line => line != "").Skip(1).Select(Update.Parse).ToArray();

var sum = updates
    .Where(update => rules.All(update.Satisfies))
    .Select(page => page.GetMiddle())
    .Sum();

Console.WriteLine(sum);

internal record Rule(int First, int Second)
{
    public static Rule Parse(string line)
    {
        var splitResult = line.Split('|');
        return new Rule(int.Parse(splitResult[0]), int.Parse(splitResult[1]));
    }
}

internal record Update(List<int> Pages)
{
    public static Update Parse(string line) => new(line.Split(',').Select(int.Parse).ToList());

    public bool Satisfies(Rule rule)
    {
        var firstIndex = Pages.IndexOf(rule.First);
        if (firstIndex < 0)
        {
            return true;
        }

        var secondsIndex = Pages.IndexOf(rule.Second);
        if(secondsIndex < 0)
        {
            return true;
        }

        return firstIndex < secondsIndex;
    }

    public int GetMiddle() => Pages[Pages.Count / 2];
}