var lines = File.ReadLines("input.txt");
byte[][] bitmasks = lines.Select(s => s.ToCharArray().Select(c => (byte)(c - 48)).ToArray()).ToArray();

byte[] oxygenGeneratorRatingBitmask = Filter(bitmasks, FilterOnMostCommonBit);
byte[] co2ScrubbingRatingBitmask = Filter(bitmasks, FilterOnLeastCommonBit);

int oxygenGeneratorRating = Convert.ToInt32(string.Concat(oxygenGeneratorRatingBitmask), 2);
int co2ScrubbingRating = Convert.ToInt32(string.Concat(co2ScrubbingRatingBitmask), 2);

Console.WriteLine(oxygenGeneratorRating * co2ScrubbingRating);

static byte[] Filter(byte[][] bitmasks, Func<byte[][], int, byte[][]> filter)
{
    byte[][] currentNumbers = bitmasks.ToArray();
    for (int bitPosition = 0; bitPosition < 12; bitPosition++)
    {
        currentNumbers = filter(currentNumbers, bitPosition);
        if(currentNumbers.Length == 1)
        {
            break;
        }
    }

    return currentNumbers.Single();
}

static byte[][] FilterOnMostCommonBit(byte[][] bitmasks, int bitPosition)
{
    int mostCommonBit = CalculateMostCommonBit(bitmasks, bitPosition);
    return bitmasks.Where(x => x[bitPosition] == mostCommonBit).ToArray();
}

static byte[][] FilterOnLeastCommonBit(byte[][] bitmasks, int bitPosition)
{
    int leastCommonBit = CalculateLeastCommonBit(bitmasks, bitPosition);
    return bitmasks.Where(x => x[bitPosition] == leastCommonBit).ToArray();
}

static int CalculateMostCommonBit(byte[][] bitmasks, int bitPosition)
{
    int numberOfZeros = bitmasks.Count(x => x[bitPosition] == 0);
    return numberOfZeros > bitmasks.Length - numberOfZeros ? 0 : 1;
}

static int CalculateLeastCommonBit(byte[][] bitmasks, int bitPosition)
{
    int numberOfZeros = bitmasks.Count(x => x[bitPosition] == 0);
    return numberOfZeros <= bitmasks.Length - numberOfZeros ? 0 : 1;
}
