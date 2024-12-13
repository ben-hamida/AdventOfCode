var sum = File
    .ReadLines("input.txt")
    .Chunk(4)
    .Select(chunk => chunk.ToArray())
    .Select(ParseGame)
    .Select(GetNumberOfPresses)
    .Sum(n => n.A * 3 + n.B);

Console.WriteLine(sum);
return;

static ((int X, int Y) A, (int X, int Y) B, (long X, long Y) P) ParseGame(string[] gameLines) =>
(
    ParseIncrements(gameLines[0]),
    ParseIncrements(gameLines[1]),
    (
        X: long.Parse(new string(gameLines[2].SkipWhile(c => c != '=').Skip(1).TakeWhile(c => c != ',').ToArray())) + 10000000000000L,
        Y: long.Parse(new string(gameLines[2].SkipWhile(c => c != 'Y').Skip(2).ToArray())) + 10000000000000L
    )
);

static (int X, int Y) ParseIncrements(string line) =>
(
    int.Parse(new string(line.SkipWhile(c => c != '+').Skip(1).TakeWhile(c => c != ',').ToArray())),
    int.Parse(new string(line.SkipWhile(c => c != ',').SkipWhile(c => c != '+').Skip(1).ToArray()))
);

static (long? A, long? B) GetNumberOfPresses(((int X, int Y) A, (int X, int Y) B, (long X, long Y) P) game)
{
    var denominator = game.A.X * game.B.Y - game.A.Y * game.B.X;
    var aNumerator = game.P.X * game.B.Y - game.B.X * game.P.Y;
    var bNumerator = game.P.Y * game.A.X - game.A.Y * game.P.X;
    return (
        aNumerator  % denominator == 0 ? aNumerator  / denominator : null,
        bNumerator  % denominator == 0 ? bNumerator  / denominator : null
    );
}