var lines = File
    .ReadLines("input.txt")
    .Select(a => a.Split("   "))
    .Select(x => (int.Parse(x[0]), int.Parse(x[1])))
    .ToArray();

var firstList = lines.Select(x => x.Item1).OrderBy(x => x);
var secondList = lines.Select(x => x.Item2).OrderBy(x => x);

var sum = firstList.Zip(secondList).Select(x => Math.Abs(x.First - x.Second)).Sum();

Console.WriteLine(sum);