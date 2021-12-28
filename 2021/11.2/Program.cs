int[,] energyLevels =
{
    { 8, 2, 5, 8, 7, 4, 1, 2, 5, 4 },
    { 3, 3, 3, 5, 2, 8, 6, 2, 1, 1 },
    { 8, 4, 6, 8, 6, 6, 1, 3, 1, 1 },
    { 6, 1, 6, 4, 5, 7, 8, 3, 5, 3 },
    { 2, 1, 3, 8, 4, 1, 4, 5, 5, 3 },
    { 1, 7, 8, 5, 3, 8, 5, 4, 4, 7 },
    { 3, 4, 4, 1, 1, 3, 3, 7, 5, 1 },
    { 3, 5, 8, 6, 8, 6, 2, 8, 3, 7 },
    { 7, 5, 6, 8, 2, 7, 2, 8, 7, 8 },
    { 6, 8, 3, 3, 6, 4, 3, 1, 4, 4 }
};

int step = 0;
while (true)
{
    step++;
    HashSet<(int x, int y)> flashed = new();

    void Increment(int x, int y)
    {
        if (x is < 0 or > 9 ||
            y is < 0 or > 9 ||
            flashed.Contains((x, y)))
        {
            return;
        }

        energyLevels[x, y]++;
        if(energyLevels[x, y] > 9)
        {
            flashed.Add((x, y));
            energyLevels[x, y] = 0;

            Increment(x, y + 1);
            Increment(x, y - 1);
            Increment(x + 1, y);
            Increment(x - 1, y);
            Increment(x + 1, y + 1);
            Increment(x + 1, y - 1);
            Increment(x - 1, y + 1);
            Increment(x - 1, y - 1);
        }
    }

    for (int x = 0; x < 10; x++)
    {
        for (int y = 0; y < 10; y++)
        {
            Increment(x, y);
        }
    }

    if(flashed.Count == 100)
    {
        break;
    }
}

Console.WriteLine(step);