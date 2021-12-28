string inputLines = File.ReadAllText("input.txt");

int[] crabPositions = inputLines.Split(',').Select(int.Parse).ToArray();

int leastFuelConsumption = Enumerable
    .Range(0, crabPositions.Max())
    .Select(targetPosition => crabPositions
        .Sum(currentCrabPosition => Math.Abs(currentCrabPosition - targetPosition)))
    .Min();

Console.WriteLine(leastFuelConsumption);