using System.Text.RegularExpressions;

var input = string.Join("", File.ReadLines("input.txt"));

var (_, sum) = Regex
    .Matches(input, @"mul\((\d\d?\d?),(\d\d?\d?)\)|do\(\)|don't\(\)")
    .Aggregate((Enabled: true, Sum: 0), (tuple, match) => match.Value switch
    {
        var value when value.StartsWith("mul") && tuple.Enabled =>
            (tuple.Enabled, tuple.Sum + int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)),
        "do()" => (true, tuple.Sum),
        "don't()" => (false, tuple.Sum),
        _ => (tuple.Enabled, tuple.Sum)
    });

Console.WriteLine(sum);