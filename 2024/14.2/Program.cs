using System.Text;

const int width = 101;
const int height = 103;

var robots = File
    .ReadLines("input.txt")
    .Select(ParseRobot)
    .ToArray();

var seconds = 0;
while (true)
{
    var positions = robots.Select(r => r.Position).ToHashSet();
    if (DetectPictureFrame(positions))
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Seconds: " + seconds);
        Console.WriteLine();
        Print(positions);
        Console.ReadLine();
    }

    robots = robots.Select(Move).ToArray();
    seconds++;
}

bool DetectPictureFrame(HashSet<(int X, int Y)> coordinates)
{
    return coordinates.GroupBy(c => c.Y).Count(g => g.Count() > 30) == 2;
}

static void Print(HashSet<(int X, int Y)> robots)
{
    var sb = new StringBuilder();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            sb.Append(robots.Contains((x, y)) ? "*" : ".");
        }

        sb.AppendLine();
    }

    Console.WriteLine(sb.ToString());
}

static Robot ParseRobot(string line)
{
    var parts = line.Split(' ');
    var positionParts = parts[0][2..].Split(',');
    var velocityParts = parts[1][2..].Split(',');
    return new Robot(
        (int.Parse(positionParts[0]), int.Parse(positionParts[1])),
        (int.Parse(velocityParts[0]), int.Parse(velocityParts[1])));
}

static Robot Move(Robot robot)
{
    var newX = (robot.Position.X + robot.Velocity.X) switch
    {
        < 0 and var n => width + n,
        >= width and var n => n - width,
        var n => n
    };

    var newY = (robot.Position.Y + robot.Velocity.Y) switch
    {
        < 0 and var n => height + n,
        >= height and var n => n - height,
        var n => n
    };

    return robot with { Position = (newX, newY) };
}

internal record Robot((int X, int Y) Position, (int X, int Y) Velocity);