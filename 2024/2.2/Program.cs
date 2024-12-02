var count = File
    .ReadLines("input.txt")
    .Select(line => line
        .Split(' ')
        .Select(int.Parse)
        .ToArray())
    .Count(IsSafe);


Console.WriteLine(count);
return;

bool IsSafe(int[] report) =>
    IsSafeInDirection(report, isDesc: true, hasRemovedUnsafeLevel: false) ||
    IsSafeInDirection(report, isDesc: false, hasRemovedUnsafeLevel: false);

bool IsSafeInDirection(int[] report, bool isDesc, bool hasRemovedUnsafeLevel)
{
    for (var i = 0; i < report.Length - 1; i++)
    {
        var diff = isDesc ? report[i] - report[i + 1] : report[i + 1] - report[i];
        if (diff is >= 1 and <= 3)
        {
            continue;
        }

        if (hasRemovedUnsafeLevel)
        {
            return false;
        }

        return IsSafeInDirection([..report[..i], ..report[(i + 1)..]], isDesc, true) ||
               IsSafeInDirection([..report[..(i + 1)], ..report[(i + 2)..]], isDesc, true);;
    }

    return true;
}
