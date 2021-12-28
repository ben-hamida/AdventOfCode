string inputLines = File.ReadAllText("input.txt");

int[] crabPositions = inputLines.Split(',').Select(int.Parse).ToArray();

int leastFuelConsumption = Enumerable
    .Range(0, crabPositions.Max())
    .Select(targetPosition => crabPositions
        .Sum(currentPosition => CalculateFuelConsumption(currentPosition, targetPosition)))
    .Min();

Console.WriteLine(leastFuelConsumption);

static int CalculateFuelConsumption(int currentPosition, int targetPosition) =>
    Enumerable.Range(1, Math.Abs(targetPosition - currentPosition)).Sum();