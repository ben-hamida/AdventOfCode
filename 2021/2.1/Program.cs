var lines = File.ReadLines("input.txt");

int depth = 0;
int horizontalPosition = 0;
foreach (string line in lines)
{
    string[] parts = line.Split(' ');
    string command = parts[0];
    int amount = int.Parse(parts[1]);
    switch (command)
    {
        case "forward":
            horizontalPosition += amount;
            break;
        case "up":
            depth -= amount;
            break;
        case "down":
            depth += amount;
            break;
    }
}

Console.WriteLine(depth * horizontalPosition);