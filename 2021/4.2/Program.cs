﻿string[] lines = File.ReadLines("input.txt").ToArray();

int[] sequence = lines.First().Split(',').Select(int.Parse).ToArray();

List<BingoBoard> bingoBoards = ParseBingoBoards(lines).ToList();

foreach (int number in sequence)
{
    foreach (BingoBoard board in bingoBoards.ToList())
    {
        bool win = board.HandleDrawnNumber(number);
        if (win)
        {
            bingoBoards.Remove(board);
            if (!bingoBoards.Any())
            {
                int score = board.CalculateScore(number);
                Console.WriteLine(score);
                return;
            }
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
    private bool _hasBingo = false;

    private BingoBoard(BingoNumber[,] grid) => _grid = grid;

    public bool HandleDrawnNumber(int drawnNumber)
    {
        if(_hasBingo)
        {
            return true;
        }

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

        _hasBingo = HasBingo();
        return _hasBingo;
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
        BingoNumber[,] grid = new BingoNumber[5, 5];

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