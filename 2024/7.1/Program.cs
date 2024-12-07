var sum = File.ReadLines("input.txt")
    .Select(line => line.Split(':'))
    .Select(splitResult => new
    {
        testValue = long.Parse(splitResult[0]),
        numbers = splitResult[1].Trim().Split(' ').Select(long.Parse).ToArray()
    })
    .Select(line => new
    {
        line.testValue,
        possibleResults = line.numbers.Skip(1).Aggregate<long, IEnumerable<long>>(
            [line.numbers[0]],
            (current, number) => current.SelectMany(x => new[] { x + number, x * number }))
    })
    .Where(t => t.possibleResults.Any(result => result == t.testValue))
    .Sum(t => t.testValue);

Console.WriteLine(sum);