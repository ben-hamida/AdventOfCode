var lines = File
    .ReadLines("input.txt")
    .Select(x => x.Split("   "))
    .Select(x => (long.Parse(x[0]), long.Parse(x[1])))
    .ToArray();

var firstList = lines.Select(x => x.Item1);
var secondList = lines.Select(x => x.Item2);

var sum = firstList
    .Select(a => (a, secondList.Count(b => a == b)))
    .Select(x => x.a * x.Item2)
    .Sum();

Console.WriteLine(sum);