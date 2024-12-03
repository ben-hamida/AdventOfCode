using System.Text.RegularExpressions;

var input = string.Join("", File.ReadLines("input.txt"));

var sum = Regex
    .Matches(input, @"mul\((\d\d?\d?),(\d\d?\d?)\)")
    .Select(m => m.Groups)
    .Aggregate(0, (i, group) => i + int.Parse(group[1].Value) * int.Parse(group[2].Value));

Console.WriteLine(sum);