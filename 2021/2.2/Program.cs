var lines = File.ReadLines("input.txt");

int depth = 0;
int horizontalPosition = 0;
int aim = 0;
foreach (var line in lines)
{
    string[] parts = line.Split(' ');
    string command = parts[0];
    int amount = int.Parse(parts[1]);
    switch (command)
    {
        case "forward":
            horizontalPosition += amount;
            depth += aim * amount;
            break;
        case "up":
            aim -= amount;
            break;
        case "down":
            aim += amount;
            break;
    }
}

Console.WriteLine(depth * horizontalPosition);