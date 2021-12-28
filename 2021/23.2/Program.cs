HashSet<Amphipod> amphipods = new()
{
    new Amphipod('A', 4, 1),
    new Amphipod('A', 8, 2),
    new Amphipod('A', 6, 3),
    new Amphipod('A', 8, 4),

    new Amphipod('B', 2, 1),
    new Amphipod('B', 6, 1),
    new Amphipod('B', 6, 2),
    new Amphipod('B', 4, 3),

    new Amphipod('C', 8, 1),
    new Amphipod('C', 4, 2),
    new Amphipod('C', 8, 3),
    new Amphipod('C', 2, 4),

    new Amphipod('D', 2, 2),
    new Amphipod('D', 2, 3),
    new Amphipod('D', 4, 4),
    new Amphipod('D', 6, 4)
};

long lowestEnergy = CalculateLowestEnergyConsumption(amphipods);

Console.WriteLine(lowestEnergy);

static long CalculateLowestEnergyConsumption(HashSet<Amphipod> init)
{
    HashSet<HashSet<Amphipod>> visitedScenarios = new(new HashSetComparer());
    PriorityQueue<HashSet<Amphipod>, long> possibleScenarios = new();
    possibleScenarios.Enqueue(init, 0);
    while (possibleScenarios.TryDequeue(out HashSet<Amphipod>? amphipods, out long consumption))
    {
        if (TargetsReached(amphipods))
        {
            return consumption;
        }

        if (!visitedScenarios.Add(amphipods))
        {
            continue;
        }

        foreach (var amphipod in amphipods)
        {
            var possibleMoves = GetAllPossibleMoves(amphipod, amphipods);
            foreach (var possibleMove in possibleMoves)
            {
                var newState = ReplaceAmphipod(amphipods, amphipod, possibleMove.amphipod);
                long newEnergyConsumption = consumption + possibleMove.numberOfSteps * GetEnergyConsumptionPerMove(amphipod);
            
                if (visitedScenarios.Contains(newState))
                {
                    // Since queue prioritizes lower consumption if we have visited this scenario before it means we did it
                    // with a lower energy consumption so we can discard this path
                    continue;
                }

                possibleScenarios.Enqueue(newState, newEnergyConsumption);           
            }
        }
    }

    return -1;
}

static HashSet<Amphipod> ReplaceAmphipod(HashSet<Amphipod> amphipods, Amphipod old, Amphipod @new)
{
    var amphipodsCopy = new HashSet<Amphipod>(amphipods);
    amphipodsCopy.Remove(old);
    amphipodsCopy.Add(@new);
    return amphipodsCopy;
}

static bool IsInTheHall(Amphipod amphipod) => amphipod.X == 0;

static bool IsInARoom(Amphipod amphipod) => amphipod.X > 0;

static int GetY(Amphipod amphipod) => amphipod.Y;

static Func<Amphipod, bool> IsToTheLeftOf(Amphipod amphipod) => a => a.Y < amphipod.Y;

static Func<Amphipod, bool> IsToTheRightOf(Amphipod amphipod) => a => a.Y > amphipod.Y;

static Func<Amphipod, bool> IsAtY(int y) => a => a.Y == y;

static Func<Amphipod, bool> IsOfType(char type) => a => a.Type == type;

static IEnumerable<(Amphipod amphipod, int numberOfSteps)> GetAllPossibleMoves(Amphipod amphipod, HashSet<Amphipod> amphipods)
{
    if (IsInARoom(amphipod))
    {
        if (amphipods.Any(a => a.Y == amphipod.Y && a.X < amphipod.X))
        {
            // Blocked and cannot move
            yield break;
        }

        int destinationY = GetTargetY(amphipod);
        if (amphipod.Y == destinationY)
        {
            if (amphipods.Where(IsAtY(destinationY)).Where(a => a.X > amphipod.X).All(IsOfType(amphipod.Type)))
            {
                // All amphipods at the target destination that are further in are of the same type, no reason to move.
                yield break;
            }           
        }

        int nearestToTheLeft = amphipods
            .Where(IsInTheHall)
            .Where(IsToTheLeftOf(amphipod))
            .MaxBy(GetY)?.Y ?? -1;
            
        int nearestToTheRight = amphipods
            .Where(IsInTheHall)
            .Where(IsToTheRightOf(amphipod))
            .MinBy(GetY)?.Y ?? 11;

        var possibleHallPositions = new[] { 0, 1, 3, 5, 7 , 9, 10 }
            .Where(y => y > nearestToTheLeft && y < nearestToTheRight);

        foreach (int yPosition in possibleHallPositions)
        {
            int stepsToGetThere = amphipod.X + Math.Abs(yPosition - amphipod.Y);
            yield return (amphipod with { Y = yPosition, X = 0 }, stepsToGetThere);
        }
    }
    else
    {
        int destinationY = GetTargetY(amphipod);
        var amphipodsAtDestination = amphipods.Where(IsAtY(destinationY)).ToArray();
        if (amphipodsAtDestination.Any(a => a.Type != amphipod.Type))
        {
            yield break;
        }

        if (destinationY < amphipod.Y) // target is to the left
        {
            bool anyAmphipodsBlocking = amphipods.Where(IsInTheHall).Where(IsToTheLeftOf(amphipod)).Any(a => destinationY < a.Y);
            if (anyAmphipodsBlocking)
            {
                yield break;
            }
        }
        else
        {
            bool anyAmphipodsBlocking = amphipods.Where(IsInTheHall).Where(IsToTheRightOf(amphipod)).Any(a => a.Y < destinationY);
            if (anyAmphipodsBlocking)
            {
                yield break;
            }
        }

        int destinationX = 4 - amphipodsAtDestination.Length;
        int stepsToGetThere = Math.Abs(destinationY - amphipod.Y) + destinationX;
        yield return (amphipod with { X = destinationX, Y = destinationY }, stepsToGetThere);
    }
}

static int GetEnergyConsumptionPerMove(Amphipod amphipod) => amphipod.Type switch
{
    'A' => 1,
    'B' => 10,
    'C' => 100,
    'D' => 1000,
    _ => throw new Exception()
};

static int GetTargetY(Amphipod amphipod) => amphipod.Type switch
{
    'A' => 2,
    'B' => 4,
    'C' => 6,
    'D' => 8,
    _ => throw new Exception()
};

static bool TargetsReached(IEnumerable<Amphipod> amphipods) => amphipods.All(IsInPlace);

static bool IsInPlace(Amphipod amphipod) => amphipod.X != 0 && amphipod.Y == GetTargetY(amphipod);

internal sealed record Amphipod(char Type, int Y, int X);

internal sealed class HashSetComparer : IEqualityComparer<HashSet<Amphipod>>
{
    public bool Equals(HashSet<Amphipod>? x, HashSet<Amphipod>? y) => x!.SetEquals(y!);

    public int GetHashCode(HashSet<Amphipod> amphipods) => amphipods
        .Aggregate(0, (current, next) => current ^ (amphipods.Comparer.GetHashCode(next) & 0x7FFFFFFF));
}