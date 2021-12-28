string text = File.ReadAllText("input.txt");

List<int> fishes = text.Split(',').Select(int.Parse).ToList();

Dictionary<int, long> timers = Enumerable.Range(0, 9).ToDictionary(x => x, x => (long)fishes.Count(y => y == x));

for (int day = 0; day < 256; day++)
{
    timers = new Dictionary<int, long>
    {
        { 0, timers[1] },
        { 1, timers[2] },
        { 2, timers[3] },
        { 3, timers[4] },
        { 4, timers[5] },
        { 5, timers[6] },
        { 6, timers[7] + timers[0] },
        { 7, timers[8] },
        { 8, timers[0] }
    };
}

Console.WriteLine(timers.Values.Sum());