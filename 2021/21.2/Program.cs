Dictionary<Game, long> ongoingGames = new() { { new Game(new Player(1, 0), new Player(6, 0)), 1 }};

var possibleRolls = new Dictionary<int, int> // The roll value mapping to the number of combinations that can result in this value
{
    { 3, 1 },
    { 4, 3 },
    { 5, 6 },
    { 6, 7 },
    { 7, 6 },
    { 8, 3 },
    { 9, 1 }
};

long numberOfWinsForPlayer1 = 0;
long numberOfWinsForPlayer2 = 0;

while (ongoingGames.Any())
{
    // Move player 1 in all games
    Dictionary<Game, long> spawnedGames = new();
    foreach ((Game game, long gameCount) in ongoingGames.ToList())
    {
        var newGames = Play(game, 1, possibleRolls);
        foreach ((Game newGame, long newGameCount) in newGames)
        {
            if (Player1Wins(newGame))
            {
                numberOfWinsForPlayer1 += gameCount * newGameCount;
            }
            else
            {
                EnsureExists(spawnedGames, newGame);
                spawnedGames[newGame] += gameCount * newGameCount;
            }
        }
    }

    ongoingGames = spawnedGames;

    // Move player 2 in all games
    spawnedGames = new();
    foreach ((Game game, long gameCount) in ongoingGames.ToList())
    {
        var newGames = Play(game, 2, possibleRolls);
        foreach ((Game newGame, long newGameCount) in newGames)
        {
            if (Player2Wins(newGame))
            {
                numberOfWinsForPlayer2 += gameCount * newGameCount;
            }
            else
            {
                EnsureExists(spawnedGames, newGame);
                spawnedGames[newGame] += gameCount * newGameCount;
            }
        }
    }

    ongoingGames = spawnedGames;
}

long mostWins = Math.Max(numberOfWinsForPlayer1, numberOfWinsForPlayer2);

Console.WriteLine(mostWins);

static void EnsureExists(IDictionary<Game, long> games, Game game)
{
    if (!games.ContainsKey(game))
    {
        games.Add(game, 0);
    }
}

static bool Player1Wins(Game game) => game.Player1.Score >= 21;

static bool Player2Wins(Game game) => game.Player2.Score >= 21;

static Dictionary<Game, long> Play(Game game, int player, Dictionary<int, int> rolls)
{
    Dictionary<Game, long> spawnedGames = new();

    foreach (var roll in rolls)
    {
        Game spawnedGame = player == 1
            ? game with { Player1 = MovePlayer(game.Player1, roll.Key) }
            : game with { Player2 = MovePlayer(game.Player2, roll.Key) };
        
        EnsureExists(spawnedGames, spawnedGame);
        spawnedGames[spawnedGame] += roll.Value;
    }

    return spawnedGames;
}

static Player MovePlayer(Player player, int amount) => UpdateScore(UpdatePosition(player, amount));

static Player UpdatePosition(Player player, int amount) => player
    with { Position = EnsureZeroIsTen((player.Position + amount) % 10) };

static Player UpdateScore(Player player) => player
    with { Score = player.Score + player.Position };

static int EnsureZeroIsTen(int position) => position == 0 ? 10 : position;

internal sealed record Player(int Position, int Score);

internal sealed record Game(Player Player1, Player Player2);
