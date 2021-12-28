string text = File.ReadAllText("input.txt");

List<int> fishes = text.Split(',').Select(int.Parse).ToList();

for (int day = 0; day < 80; day++)
{
    int numberOfNewFishes = 0;
    for (int fishIndex = 0; fishIndex < fishes.Count; fishIndex++)
    {
        int currentValue = fishes[fishIndex];
        if(currentValue == 0)
        {
            numberOfNewFishes++;
            fishes[fishIndex] = 6;
        }
        else
        {
            fishes[fishIndex] = currentValue - 1;
        }
    }

    fishes.AddRange(Enumerable.Repeat(8, numberOfNewFishes));
}

Console.WriteLine(fishes.Count);