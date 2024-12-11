List<long> stones = [8069, 87014, 98, 809367, 525, 0, 9494914, 5];

for (var i = 0; i < 25; i++)
{
    for (var j = 0; j < stones.Count; j++)
    {
        if (stones[j] == 0)
        {
            stones[j] = 1;
            continue;
        }

        var numberString = stones[j].ToString();
        if (numberString.Length % 2 == 0)
        {
            stones.Insert(j, long.Parse(numberString[..(numberString.Length / 2)]));
            stones[j + 1] = long.Parse(numberString[(numberString.Length / 2)..]);
            j++;
            continue;
        }

        stones[j] *= 2024;
    }
}

Console.WriteLine(stones.Count);