var inputLines = File.ReadLines("input.txt");

int[] uniqueNumberOfDigitSegments = { 2 , 4, 3, 7 };

int numberOfOccurrences = inputLines
    .Select(ParseDigits)
    .SelectMany(x => x)
    .Count(x => uniqueNumberOfDigitSegments.Contains(x.Length));

Console.WriteLine(numberOfOccurrences);

static IEnumerable<string> ParseDigits(string line) => line.Split('|')[1].Trim().Split(' ');