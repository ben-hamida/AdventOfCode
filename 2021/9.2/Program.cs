string[] lines = File.ReadLines("input.txt").ToArray();

byte[,] heightMap = ParseHeightMap(lines);

int rows = heightMap.GetUpperBound(0) + 1;
int columns = heightMap.GetUpperBound(1) + 1;

var basinSizes = new List<int>();
var includedInBasin = new HashSet<(int, int)>();
for (int row = 0; row < rows; row++)
{
    for (int column = 0; column < columns; column++)
    {
        if (includedInBasin.Contains((row, column)) || heightMap[row, column] == 9)
        {
            continue;
        }

        basinSizes.Add(CalculateBasinSize(row, column));
    }
}

int result = basinSizes.OrderByDescending(x => x).Take(3).Aggregate((current, next) => current * next);

Console.WriteLine(result);

int CalculateBasinSize(int row, int column)
{
    if (row < 0 ||
        column < 0 ||
        row >= rows ||
        column >= columns ||
        heightMap[row, column] == 9 ||
        includedInBasin.Contains((row, column)))
    {
        return 0;
    }

    includedInBasin.Add((row, column));

    return 1 +
           CalculateBasinSize(row - 1, column) +
           CalculateBasinSize(row + 1, column) +
           CalculateBasinSize(row, column - 1) +
           CalculateBasinSize(row, column + 1);
}

static byte[,] ParseHeightMap(IReadOnlyList<string> lines)
{
    var heightMap = new byte[lines.Count, lines[0].Length];

    for (int row = 0; row <= heightMap.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= heightMap.GetUpperBound(1); column++)
        {
            heightMap[row, column] = (byte)(lines[row][column] - 48);
        }
    }

    return heightMap;
}
