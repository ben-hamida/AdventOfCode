var lines = File.ReadLines("input.txt");

var occurrence = new int[12];
foreach(int[] bitmask in lines.Select(s => s.ToCharArray().Select(c => c - 48).ToArray()))
{
    for(int i = 0; i < 12; i++)
    {
        if(bitmask[i] == 0)
        {
            occurrence[i] -= 1;
        }
        else
        {
            occurrence[i] += 1;
        }
    }
}

var gammaBitmask = new byte[12];
var epsilonBitmask = new byte[12];

for (int i = 0; i < 12; i++)
{
    if(occurrence[i] < 0)
    {
        epsilonBitmask[i] = 1;
    }
    else
    {
        gammaBitmask[i] = 1;
    }
}

int gammaRate = Convert.ToInt32(string.Concat(gammaBitmask), 2);
int epsilonRate = Convert.ToInt32(string.Concat(epsilonBitmask), 2);

Console.WriteLine(gammaRate * epsilonRate);
