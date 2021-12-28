string[] lines = File.ReadLines("input.txt").ToArray();

byte[,] heightMap = ParseHeightMap(lines);

int rows = heightMap.GetUpperBound(0) + 1;
int columns = heightMap.GetUpperBound(1) + 1;

int totalRiskLevel = 0;
for (int row = 0; row < rows; row++)
{
    for (int column = 0; column < columns; column++)
    {
        int height = heightMap[row, column];
        if ((row - 1 < 0            || heightMap[row - 1, column] > height) &&
            (row + 1 >= rows        || heightMap[row + 1, column] > height) &&
            (column - 1 < 0         || heightMap[row, column - 1] > height) &&
            (column + 1 >= columns  || heightMap[row, column + 1] > height))
        {
            totalRiskLevel += height + 1;
        }
    }
}

Console.WriteLine(totalRiskLevel);

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