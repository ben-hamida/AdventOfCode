string[] lines = File.ReadLines("input.txt").ToArray();

var sequence = lines.First().Split(',').Select(int.Parse);

BingoBoard[] bingoBoards = ParseBingoBoards(lines).ToArray();

foreach (int number in sequence)
{
    foreach (BingoBoard board in bingoBoards)
    {
        bool win = board.HandleDrawnNumber(number);
        if (win)
        {
            int score = board.CalculateScore(number);
            Console.WriteLine(score);
            return;
        }
    }
}

static IEnumerable<BingoBoard> ParseBingoBoards(string[] lines)
{
    int lineNumber = 1;
    while(lineNumber < lines.Length)
    {
        lineNumber++;
        yield return BingoBoard.Parse(lines, lineNumber);
        lineNumber += 5;
    }
}

internal class BingoBoard
{
    private readonly BingoNumber[,] _grid;

    private BingoBoard(BingoNumber[,] grid) => _grid = grid;

    public bool HandleDrawnNumber(int drawnNumber)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (_grid[row, column].Number == drawnNumber)
                {
                    _grid[row, column].Mark();
                }
            }
        }

        return HasBingo();
    }

    public int CalculateScore(int finalNumber)
    {
        int sum = 0;
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (!_grid[row, column].IsMarked)
                {
                    sum += _grid[row, column].Number;
                }
            }
        }

        return sum * finalNumber;
    }

    private bool HasBingo() => HasBingoInARow() || HasBingoInAColumn();

    private bool HasBingoInARow()
    {
        for (int row = 0; row < 5; row++)
        {
            bool allNumbersMarked = true;
            for (int column = 0; column < 5; column++)
            {
                if (!_grid[row, column].IsMarked)
                {
                    allNumbersMarked = false;
                }
            }

            if (allNumbersMarked)
            {
                return true;
            }
        }

        return false;
    }

    
    private bool HasBingoInAColumn()
    {
        for (int column = 0; column < 5; column++)
        {
            bool allNumbersMarked = true;
            for (int row = 0; row < 5; row++)
            {
                if (!_grid[row, column].IsMarked)
                {
                    allNumbersMarked = false;
                }
            }

            if (allNumbersMarked)
            {
                return true;
            }
        }

        return false;
    }

    public static BingoBoard Parse(string[] lines, int position)
    {
        var grid = new BingoNumber[5, 5];

        for (int row = 0; row < 5; row++)
        { 
            int[] rowNumbers = lines[position + row].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
            for (int column = 0; column < 5; column++)
            {
                grid[row, column] = new BingoNumber(rowNumbers[column]);
            }
        }

        return new BingoBoard(grid);
    }
}

internal class BingoNumber
{
    public BingoNumber(int number) => Number = number;

    public int Number {get;}

    public bool IsMarked{ get; private set; }

    public void Mark() => IsMarked = true;
}