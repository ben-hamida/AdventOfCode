int player1Position = 1;
int player2Position = 6;

int player1Score = 0;
int player2Score = 0;

Die die = new();
while (true)
{
    (player1Position, player1Score) = Play(player1Position, player1Score, die);
    if (player1Score >= 1000)
    {
        break;
    }

    (player2Position, player2Score) = Play(player2Position, player2Score, die);
    if (player2Score >= 1000)
    {
        break;
    }
}

int losingPlayerScore = Math.Min(player1Score, player2Score);
int result = losingPlayerScore * die.TotalNumberOfRolls;

Console.WriteLine(result);

static (int newPosition, int newScore) Play(int playerPosition, int score, Die die)
{
    int dieResult = Roll3Times(die);
    int newPosition = MovePlayer(playerPosition, dieResult);
    return (newPosition, score += newPosition);
}

static int Roll3Times(Die die) => die.Roll() + die.Roll() + die.Roll();

static int MovePlayer(int currentPosition, int amount) => EnsureZeroIsTen((currentPosition + amount) % 10);

static int EnsureZeroIsTen(int position) => position == 0 ? 10 : position;

internal sealed class Die
{
    private int _currentValue;

    public int TotalNumberOfRolls { get; private set; }

    public int Roll()
    {
        TotalNumberOfRolls++;
        _currentValue = (_currentValue + 1) % 100;
        return _currentValue;
    }
}