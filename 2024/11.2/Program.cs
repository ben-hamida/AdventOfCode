var count = Enumerable
    .Range(0, 75)
    .Aggregate(
        new long[] { 8069, 87014, 98, 809367, 525, 0, 9494914, 5 }.Select(number => (Number: number, Count: 1L)),
        (current, _) => current
            .SelectMany<(long Number, long Count), (long Number, long Count)>(stone => stone switch
            {
                (0, var count) => [(1L, count)],
                var (number, count) when number.ToString().Length % 2 == 0 =>
                [
                    (long.Parse(number.ToString()[..(number.ToString().Length / 2)]), count),
                    (long.Parse(number.ToString()[(number.ToString().Length / 2)..]), count)
                ],
                var (number, count) => [(number * 2024, count)]
            })
            .GroupBy(stone => stone.Number)
            .Select(grouping => (Number: grouping.Key, Count: grouping.Sum(stone => stone.Count))))
    .Sum(stone => stone.Count);

Console.WriteLine(count); // 218817038947400