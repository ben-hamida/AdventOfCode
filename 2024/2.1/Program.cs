var count = File
    .ReadLines("input.txt")
    .Select(line => line
        .Split(' ')
        .Select(int.Parse)
        .ToArray())
    .Count(IsSafe);

Console.WriteLine(count);
return;

bool IsSafe(int[] report) => IsSafeInDirection(report, isDesc: false) || IsSafeInDirection(report, isDesc: true);

bool IsSafeInDirection(int[] report, bool isDesc)
{
    for (var i = 0; i < report.Length - 1; i++)
    {
        var diff = isDesc ? report[i] - report[i + 1] : report[i + 1] - report[i];
        if (diff is < 1 or > 3)
        {
            return false;
        }
    }

    return true;
}