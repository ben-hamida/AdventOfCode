string[] inputLines = File.ReadLines("input.txt").ToArray();

int sum = inputLines.Select(ParseEntry).Sum(CalculateValue);

Console.WriteLine(sum);

static int CalculateValue((string[] signalPattern, string[] output) entry)
{
    (string[] signalPattern, string[] output) = entry;
    (string Segments, int Value)[] solutions = CalculateSolutions(signalPattern).ToArray();
    IEnumerable<int> values = output.Select(o => solutions.First(d => SegmentEquals(d.Segments, o)).Value);
    return int.Parse(string.Concat(values));
}

static IEnumerable<(string Segments, int Value)> CalculateSolutions(string[] digits)
{
    string GetSegmentsOfLength(int length) => digits.First(d => d.Length == length);
    string oneSegments = GetSegmentsOfLength(2);
    string fourSegments = GetSegmentsOfLength(4);
    yield return (oneSegments, 1);
    yield return (GetSegmentsOfLength(3), 7);
    yield return (fourSegments, 4);
    yield return (GetSegmentsOfLength(7), 8);

    foreach (string digit in digits)
    {
        switch (digit.Length)
        {
            case 5 when oneSegments.All(digit.Contains):
                yield return (digit, 3);
                break;
            case 5 when digit.Concat(fourSegments).Distinct().Count() == 7: // 4 and 2 should cover all segments
                yield return (digit, 2);
                break;
            case 5:
                yield return (digit, 5);
                break;
            case 6 when fourSegments.All(digit.Contains):
                yield return (digit, 9);
                break;
            case 6 when !oneSegments.All(digit.Contains):
                yield return (digit, 6);
                break;
            case 6:
                yield return (digit, 0);
                break;
        }
    }
}

static (string[] SignalPattern, string[] Output) ParseEntry(string line)
{
    string[] parts = line.Split('|');
    return (ParseDigits(parts[0]), ParseDigits(parts[1]));
}

static string[] ParseDigits(string input) => input.Trim().Split(' ').ToArray();

static bool SegmentEquals(string first, string second) =>
    first.Length == second.Length && first.All(second.Contains);
