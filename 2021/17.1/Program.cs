const int targetMinX = 257;
const int targetMaxX = 286;
const int targetMinY = -101;
const int targetMaxY = -57;

int highestYPosition = 0;

for (int xVelocity = 1; xVelocity < 287; xVelocity++)
{
    for (int yVelocity = -102; yVelocity < 1000; yVelocity++)
    {
        bool hit = SimulateLaunch((xVelocity, yVelocity), out int highestYPositionForLaunch);
        if (hit)
        {
            highestYPosition = Math.Max(highestYPosition, highestYPositionForLaunch);
        }
    }
}

Console.WriteLine(highestYPosition);

static bool SimulateLaunch((int x, int y) initialVelocity, out int highestYPosition)
{
    (int x, int y) currentVelocity = initialVelocity;
    (int x, int y) currentPosition = (0, 0);
    highestYPosition = currentPosition.y;

    while (true)
    {
        if (currentPosition.x > targetMaxX ||
            currentPosition.y < targetMinY)
        {
            return false; // Missed
        }

        if (currentPosition.x is >= targetMinX and <= targetMaxX &&
            currentPosition.y is >= targetMinY and <= targetMaxY)
        {
            return true; // Hit
        }

        currentPosition = (currentPosition.x + currentVelocity.x, currentPosition.y + currentVelocity.y);
        currentVelocity = (Math.Max(0, currentVelocity.x - 1), currentVelocity.y - 1); // Not accounting to negative x-velocity but we should never have that
        highestYPosition = Math.Max(currentPosition.y, highestYPosition);
    }
}