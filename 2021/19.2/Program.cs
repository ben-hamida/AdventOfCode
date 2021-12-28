string[] lines = File.ReadLines("input.txt").ToArray();

int currentScanner = 0;
Dictionary<int, List<(int x, int y, int z)>> scanners = new();

HashSet<Func<(int x, int y, int z), (int x, int y, int z)>> rotationTransforms = new()
{
    c => (c.x, c.y, c.z),
    c => (-c.x, c.y, c.z),
    c => (c.x, -c.y, c.z),
    c => (c.x, c.y, -c.z),
    c => (-c.x, -c.y, c.z),
    c => (-c.x, c.y, -c.z),
    c => (c.x, -c.y, -c.z),
    c => (-c.x, -c.y, -c.z),
    
    c => (c.x, c.z, c.y),
    c => (-c.x, c.z, c.y),
    c => (c.x, -c.z, c.y),
    c => (c.x, c.z, -c.y),
    c => (-c.x, -c.z, c.y),
    c => (-c.x, c.z, -c.y),
    c => (c.x, -c.z, -c.y),
    c => (-c.x, -c.z, -c.y),

    c => (c.y, c.x, c.z),
    c => (-c.y, c.x, c.z),
    c => (c.y, -c.x, c.z),
    c => (c.y, c.x, -c.z),
    c => (-c.y, -c.x, c.z),
    c => (-c.y, c.x, -c.z),
    c => (c.y, -c.x, -c.z),
    c => (-c.y, -c.x, -c.z),
    
    c => (c.y, c.z, c.x),
    c => (-c.y, c.z, c.x),
    c => (c.y, -c.z, c.x),
    c => (c.y, c.z, -c.x),
    c => (-c.y, -c.z, c.x),
    c => (-c.y, c.z, -c.x),
    c => (c.y, -c.z, -c.x),
    c => (-c.y, -c.z, -c.x),
    
    c => (c.z, c.x, c.y),
    c => (-c.z, c.x, c.y),
    c => (c.z, -c.x, c.y),
    c => (c.z, c.x, -c.y),
    c => (-c.z, -c.x, c.y),
    c => (-c.z, c.x, -c.y),
    c => (c.z, -c.x, -c.y),
    c => (-c.z, -c.x, -c.y),
    
    c => (c.z, c.y, c.x),
    c => (-c.z, c.y, c.x),
    c => (c.z, -c.y, c.x),
    c => (c.z, c.y, -c.x),
    c => (-c.z, -c.y, c.x),
    c => (-c.z, c.y, -c.x),
    c => (c.z, -c.y, -c.x),
    c => (-c.z, -c.y, -c.x),
};

foreach (string line in lines)
{
    if (line.StartsWith("--"))
    {
        currentScanner = int.Parse(line.Substring(12, 2).TrimEnd());
        scanners[currentScanner] = new ();
        continue;
    }

    if(string.IsNullOrWhiteSpace(line))
    {
        continue;
    }

    string[] parts = line.Split(',');
    scanners[currentScanner].Add(new (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
}

Dictionary<int, (int x, int y, int z)> transformedScanners = new() { { 0, (0, 0, 0) } };

while (transformedScanners.Count != scanners.Count)
{
    foreach (int referenceScanner in transformedScanners.Keys.ToList())
    {
        foreach (int untransformedScanner in scanners.Keys.Where(scanner => !transformedScanners.ContainsKey(scanner)))
        {
            if (IsOverlap(
                referenceScanner,
                untransformedScanner,
                out var transformedSecondScannerPoints,
                out var transformedScannerPosition))
            {
                scanners[untransformedScanner] = transformedSecondScannerPoints;
                transformedScanners.Add(untransformedScanner, transformedScannerPosition);
            }
        }
    }
}

var scannerPositions = transformedScanners.Select(x => x.Value).ToArray();
int maxDistance = 0;
for (int i = 0; i < scannerPositions.Length; i++)
{
    for (int j = i + 1; j < scannerPositions.Length; j++)
    {
        int distance = Math.Abs(scannerPositions[j].x - scannerPositions[i].x) +
                       Math.Abs(scannerPositions[j].y - scannerPositions[i].y) +
                       Math.Abs(scannerPositions[j].z - scannerPositions[i].z);

        maxDistance = Math.Max(maxDistance, distance);
    }
}

Console.WriteLine(maxDistance);

bool IsOverlap(
    int referenceScanner,
    int secondScanner,
    out List<(int x, int y, int z)> transformedSecondScannerPoints,
    out (int x, int y, int z) transformedSecondScannerPosition)
{
    var referenceScannerPoints = scanners[referenceScanner];
    var secondScannerPoints = scanners[secondScanner];

    foreach (var firstScannerPoint in referenceScannerPoints)
    {
        foreach (var rotationTransform in rotationTransforms)
        {
            var rotatedSecondScannerPoints = secondScannerPoints.Select(rotationTransform);
            foreach (var secondScannerPoint in rotatedSecondScannerPoints)
            {
                // Assume the points overlap
                var translationTransform = GetTranslationTransform(firstScannerPoint, secondScannerPoint);

                transformedSecondScannerPoints = rotatedSecondScannerPoints.Select(translationTransform).ToList();

                // If assumption is correct and we find at least 12 overlapping points it's a match
                int numberOfOverlappingPoints = transformedSecondScannerPoints.Count(p => referenceScannerPoints.Contains(p));
                if (numberOfOverlappingPoints >= 12)
                {
                    transformedSecondScannerPosition = translationTransform((0, 0, 0));
                    return true;
                }
            }
        } 
    }

    transformedSecondScannerPoints = new();
    transformedSecondScannerPosition = default;
    return false;
}

static Func<(int x, int y, int z), (int x, int y, int z)> GetTranslationTransform((int x, int y, int z) first, (int x, int y, int z) second)
{
    return c => (c.x + (first.x - second.x), c.y + (first.y - second.y), c.z + (first.z - second.z));
}
