var lines = File.ReadLines("input.txt").ToArray();
var towelPatterns = lines[0].Split(", ").ToHashSet();
var designs = lines.Skip(2).ToArray();

var possibleDesignsCount = designs.Count(design => IsPossible(design, towelPatterns));

Console.WriteLine(possibleDesignsCount);
return;

static bool IsPossible(string design, HashSet<string> towelPatterns)
{
    var queue = new PriorityQueue<string, int>([(design, design.Length)]);
    var checkedDesigns = new HashSet<string>();
    while (queue.TryDequeue(out var remainingDesign, out _))
    {
        if (remainingDesign.Length == 0)
        {
            return true;
        }

        if (!checkedDesigns.Add(remainingDesign))
        {
            continue;
        }

        queue.EnqueueRange(towelPatterns
            .Where(pattern => remainingDesign.StartsWith(pattern))
            .Select(pattern => (remainingDesign[pattern.Length..], remainingDesign.Length - pattern.Length)));
    }

    return false;
}