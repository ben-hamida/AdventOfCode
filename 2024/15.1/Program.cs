var lines = File.ReadLines("input.txt").ToArray();
var mapLines = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
var movements = string.Join("", lines.Skip(mapLines.Length + 1));
var map = mapLines
    .Index()
    .SelectMany(x => x.Item.Index().Select(y => (X: x.Index, Y: y.Index, Value: y.Item)))
    .ToDictionary(a => (a.X, a.Y), a => a.Value);

var robot = map.Single(kvp => kvp.Value == '@').Key;

foreach (var movement in movements)
{
    switch (movement)
    {
        case '^':
        {
            var next = map[(robot.X - 1, robot.Y)];
            switch (next)
            {
                case '#': continue;
                case '.':
                    map[robot] = '.';
                    robot = robot with { X = robot.X - 1 };
                    map[robot] = '@';
                    continue;
            }

            var boxesNext = map
                .Where(kvp => kvp.Key.Y == robot.Y && kvp.Key.X < robot.X)
                .OrderByDescending(kvp => kvp.Key.X)
                .TakeWhile(kvp => kvp.Value == 'O')
                .ToArray();

            if (map[boxesNext[^1].Key with { X = boxesNext[^1].Key.X - 1 }] == '#')
            {
                continue;
            }

            foreach (var box in boxesNext)
            {
                map[box.Key with { X = box.Key.X - 1 }] = 'O';
            }

            map[robot] = '.';
            robot = robot with { X = robot.X - 1 };
            map[robot] = '@';
            break;
        }
        case 'v':
        {
            var next = map[(robot.X + 1, robot.Y)];
            switch (next)
            {
                case '#': continue;
                case '.':
                    map[robot] = '.';
                    robot = robot with { X = robot.X + 1 };
                    map[robot] = '@';
                    continue;
            }

            var boxesNext = map
                .Where(kvp => kvp.Key.Y == robot.Y && kvp.Key.X > robot.X)
                .OrderBy(kvp => kvp.Key.X)
                .TakeWhile(kvp => kvp.Value == 'O')
                .ToArray();

            if (map[boxesNext[^1].Key with { X = boxesNext[^1].Key.X + 1 }] == '#')
            {
                continue;
            }

            foreach (var box in boxesNext)
            {
                map[box.Key with { X = box.Key.X + 1 }] = 'O';
            }

            map[robot] = '.';
            robot = robot with { X = robot.X + 1 };
            map[robot] = '@';
            break;
        }
        case '<':
        {
            var next = map[(robot.X, robot.Y - 1)];
            switch (next)
            {
                case '#': continue;
                case '.':
                    map[robot] = '.';
                    robot = robot with { Y = robot.Y - 1 };
                    map[robot] = '@';
                    continue;
            }

            var boxesNext = map
                .Where(kvp => kvp.Key.X == robot.X && kvp.Key.Y < robot.Y)
                .OrderByDescending(kvp => kvp.Key.Y)
                .TakeWhile(kvp => kvp.Value == 'O')
                .ToArray();

            if (map[boxesNext[^1].Key with { Y = boxesNext[^1].Key.Y - 1 }] == '#')
            {
                continue;
            }

            foreach (var box in boxesNext)
            {
                map[box.Key with { Y = box.Key.Y - 1 }] = 'O';
            }

            map[robot] = '.';
            robot = robot with { Y = robot.Y - 1 };
            map[robot] = '@';
            break;
        }
        case '>':
        {
            var next = map[(robot.X, robot.Y + 1)];
            switch (next)
            {
                case '#': continue;
                case '.':
                    map[robot] = '.';
                    robot = robot with { Y = robot.Y + 1 };
                    map[robot] = '@';
                    continue;
            }

            var boxesNext = map
                .Where(kvp => kvp.Key.X == robot.X && kvp.Key.Y > robot.Y)
                .OrderBy(kvp => kvp.Key.Y)
                .TakeWhile(kvp => kvp.Value == 'O')
                .ToArray();

            if (map[boxesNext[^1].Key with { Y = boxesNext[^1].Key.Y + 1 }] == '#')
            {
                continue;
            }

            foreach (var box in boxesNext)
            {
                map[box.Key with { Y = box.Key.Y + 1 }] = 'O';
            }

            map[robot] = '.';
            robot = robot with { Y = robot.Y + 1 };
            map[robot] = '@';
            break;
        }
    }
}

Console.WriteLine(map.Where(kvp => kvp.Value == 'O').Sum(kvp => 100 * kvp.Key.X + kvp.Key.Y));