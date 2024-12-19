using System.Collections.Concurrent;

var lines = File.ReadLines("input.txt").ToArray();
var towelPatterns = lines[0].Split(", ").ToHashSet();
var designs = lines.Skip(2).ToArray();

var numberOfArrangements = designs.Sum(design => GetNumberOfArrangements("", design, towelPatterns, []));

Console.WriteLine(numberOfArrangements);
return;

static long GetNumberOfArrangements(
    string arrangement,
    string targetDesign,
    HashSet<string> towelPatterns,
    ConcurrentDictionary<string, long> cache) =>
    arrangement switch
    {
        _ when arrangement == targetDesign => 1,
        _ when arrangement.Length >= targetDesign.Length => 0,
        _ => cache.GetOrAdd(
            arrangement,
            arrangement => towelPatterns
                .Select(pattern => arrangement + pattern)
                .Where(targetDesign.StartsWith)
                .Sum(nextArrangement => GetNumberOfArrangements(nextArrangement, targetDesign, towelPatterns, cache)))
    };