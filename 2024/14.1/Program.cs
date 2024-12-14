const int width = 101;
const int height = 103;

var robots = File.ReadLines("input.txt")
    .Select(ParseRobot).ToList();

for (var i = 0; i < 100; i++)
{
    robots = robots.Select(Move).ToList();
}

var robotsInFirstQuadrant = robots.Count(r=> r.Position is { X: < (width - 1) / 2, Y: < (height - 1) / 2 });
var robotsInSecondQuadrant = robots.Count(r=> r.Position is { X: > (width - 1) / 2, Y: < (height - 1) / 2 });
var robotsInThirdQuadrant = robots.Count(r=> r.Position is { X: < (width - 1) / 2, Y: > (height - 1) / 2 });
var robotsInFourthQuadrant = robots.Count(r=> r.Position is { X: > (width - 1) / 2, Y: > (height - 1) / 2 });

Console.WriteLine(robotsInFirstQuadrant * robotsInSecondQuadrant * robotsInThirdQuadrant * robotsInFourthQuadrant);

return;

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