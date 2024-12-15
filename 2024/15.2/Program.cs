string[] lines = File.ReadLines("input.txt").ToArray();

var mapLines = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
var movements = string
    .Join("", lines.Skip(mapLines.Length + 1))
    .Select(movement => movement switch
    {
        '^' => Direction.Up,
        'v' => Direction.Down,
        '<' => Direction.Left,
        '>' => Direction.Right,
        _ => throw new ArgumentOutOfRangeException()
    })
    .ToArray();

var map = mapLines
    .Index()
    .SelectMany(x => x.Item.Index().Select(y => (X: x.Index, Y: y.Index, Value: y.Item)))
    .ToArray();

var robot = map.Where(kvp => kvp.Value == '@').Select(a => (a.X, Y: a.Y * 2)).Single();

var tiles = map
    .Where(a => a.Value is '#' or 'O')
    .Select<(int X, int Y, char Value), Tile>(a => a.Value switch
    {
        '#' => new Wall(a.X, a.Y * 2),
        'O' => new Box(a.X, a.Y * 2),
        _ => throw new ArgumentOutOfRangeException()
    })
    .ToArray();

foreach (var movement in movements)
{
    var nextPosition = GetNextRobotPosition(robot.X, robot.Y, movement);
    var obstructions = tiles.Where(tile => IsOccupying(nextPosition.X, nextPosition.Y, tile)).ToList();
    if (obstructions.All(tile => tile.CanMove(movement, tiles)))
    {
        robot = nextPosition;
        obstructions.ForEach(tile => tile.Move(movement, tiles));
    }
}

Console.WriteLine(tiles.OfType<Box>().Sum(box => box.Gps));
return;

bool IsOccupying(int x, int y, Tile tile) => tile.X == x && (tile.Y == y || tile.Y == y - 1);

static (int X, int Y) GetNextRobotPosition(int x, int y, Direction direction) => direction switch
{
    Direction.Left => (x, y - 1),
    Direction.Up => (x - 1, y),
    Direction.Right => (x, y + 1),
    Direction.Down => (x + 1, y),
    _ => throw new ArgumentOutOfRangeException(nameof(direction))
};

internal enum Direction { Left, Up, Right, Down }

internal abstract class Tile(int x, int y)
{
    public int X { get; protected set; } = x;
    public int Y { get; protected set; } = y;

    public bool IsInTheWayOf(int x, int y, Direction direction) => direction switch
    {
        Direction.Left => X == x && Y == y - 2,
        Direction.Right => X == x && Y == y + 2,
        Direction.Up => X == x - 1 && (Y == y - 1 || Y == y || Y == y + 1),
        Direction.Down => X == x + 1 && (Y == y - 1 || Y == y || Y == y + 1),
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public abstract bool CanMove(Direction direction, Tile[] tiles);

    public abstract void Move(Direction direction, Tile[] tiles);
}

internal class Wall(int x, int y) : Tile(x, y)
{
    public override bool CanMove(Direction direction, Tile[] tiles) => false;

    public override void Move(Direction direction, Tile[] tiles) => throw new InvalidOperationException();
}

internal class Box(int x, int y) : Tile(x, y)
{
    public int Gps => 100 * X + Y;

    public override bool CanMove(Direction direction, Tile[] tiles) => tiles
        .Where(tile => tile.IsInTheWayOf(X, Y, direction))
        .All(tile => tile.CanMove(direction, tiles));

    public override void Move(Direction direction, Tile[] tiles)
    {
        tiles
            .Where(tile => tile.IsInTheWayOf(X, Y, direction))
            .ToList()
            .ForEach(tile => tile.Move(direction, tiles));

        switch (direction)
        {
            case Direction.Up: X--; break;
            case Direction.Down: X++; break;
            case Direction.Left: Y--; break;
            case Direction.Right: Y++; break;
            default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}