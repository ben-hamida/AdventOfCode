string[] lines = File.ReadLines("input.txt").ToArray();

Pair[] pairs = lines.Select(Parse).ToArray();
int maxMagnitude = 0;
for (int i = 0; i < pairs.Length; i++)
{
    for (int j = 0; j < pairs.Length; j++)
    {
        if (i != j)
        {
            Pair sum = Sum(Parse(lines[i]), Parse(lines[j]));
            maxMagnitude = Math.Max(maxMagnitude, sum.CalculateMagnitude());
        }
    }
}

Console.WriteLine(maxMagnitude);

static Pair Sum(IPart first, IPart second)
{
    var sum = new Pair(first, second);
    Reduce(sum);
    return sum;
}

static void Reduce(Pair pair)
{
    bool hasChanged;
    do
    {
        hasChanged = false;
        hasChanged |= Explode(pair);
        hasChanged |= Split(pair, out pair);
    }
    while (hasChanged);
}

static bool Explode(Pair pair)
{
    bool hasExploded = false;
    while (true)
    {
        var firstFourthLevelPair = pair.GetPairs(1).FirstOrDefault(p => p.level == 4).pair;
        if (firstFourthLevelPair == null)
        {
            return hasExploded;
        }

        hasExploded = true;
        var numbers = pair.GetNumbers().ToList();

        var leftNumber = (Number)firstFourthLevelPair.Left;
        int leftNumberIndex = numbers.IndexOf(leftNumber);
        if (leftNumberIndex > 0)
        {
            numbers[leftNumberIndex - 1].AddValue(leftNumber.Value);
        }

        var rightNumber = (Number)firstFourthLevelPair.Right;
        int rightNumberIndex = numbers.IndexOf(rightNumber);
        if (rightNumberIndex < numbers.Count - 1)
        {
            numbers[rightNumberIndex + 1].AddValue(rightNumber.Value);
        }

        firstFourthLevelPair.Parent!.ReplacePart(firstFourthLevelPair, new Number(0));
    }
}

static bool Split(Pair pair, out Pair newTopPair)
{
    newTopPair = pair;
    var numberToSplit = pair.GetNumbers().FirstOrDefault(n => n.Value >= 10);
    if (numberToSplit == null)
    {
        return false;
    }

    var newPair = new Pair(
        new Number(numberToSplit.Value / 2),
        new Number(numberToSplit.Value / 2 + numberToSplit.Value % 2));

    if (numberToSplit.Parent == null)
    {
        newTopPair = newPair;
        return true;
    }

    numberToSplit.Parent.ReplacePart(numberToSplit, newPair);
    return true;
}

static Pair Parse(string input)
{
    IPart left;
    if (input[1] == '[')
    {
        int closingBracketIndex = FindClosingBracket(input, 1);
        left = Parse(input.Substring(1, closingBracketIndex));
    }
    else
    {
        left = new Number(input[1] - 48);
    }

    IPart right;
    if (input[^2] == ']')
    {
        int openingBracketIndex = FindOpeningBracket(input, input.Length - 2);
        right = Parse(input.Substring(openingBracketIndex, input.Length - openingBracketIndex - 1));
    }
    else
    {
        right = new Number(input[^2] - 48);
    }

    var pair = new Pair(left, right);
    return pair;
}

static int FindClosingBracket(string input, int openBracketIndex)
{
    int unclosedBrackets = 0;
    for (int i = openBracketIndex + 1; i < input.Length; i++)
    {
        switch (input[i])
        {
            case '[':
                unclosedBrackets++;
                break;
            case ']' when unclosedBrackets == 0:
                return i;
            case ']':
                unclosedBrackets--;
                break;
        }
    }

    throw new InvalidOperationException("Couldn't find closing bracket");
}

static int FindOpeningBracket(string input, int closeBracketIndex)
{
    int unopenedBrackets = 0;
    for (int i = closeBracketIndex - 1; i >= 0; i--)
    {
        switch (input[i])
        {
            case ']':
                unopenedBrackets++;
                break;
            case '[' when unopenedBrackets == 0:
                return i;
            case '[':
                unopenedBrackets--;
                break;
        }
    }

    throw new InvalidOperationException("Couldn't find closing bracket");
}

internal interface IPart
{
    Pair? Parent { get; set; }

    int CalculateMagnitude();

    IEnumerable<Number> GetNumbers();
}

internal sealed class Pair : IPart
{
    public Pair(IPart left, IPart right)
    {
        SetLeft(left);
        SetRight(right);
    }

    public IPart Left { get; private set; } = null!;

    public IPart Right { get; private set; } = null!;

    public Pair? Parent { get; set; }

    public void ReplacePart(IPart oldPart, IPart newPart)
    {
        if (Left == oldPart)
        {
            SetLeft(newPart);
        }
        else if (Right == oldPart)
        {
            SetRight(newPart);
        }
        else
        {
            throw new InvalidOperationException("Part doesn't exist");
        }
    }

    public void SetLeft(IPart left)
    {
        left.Parent = this;
        Left = left;
    }

    public void SetRight(IPart right)
    {
        right.Parent = this;
        Right = right;
    }

    public int CalculateMagnitude() => 3 * Left.CalculateMagnitude() + 2 * Right.CalculateMagnitude();

    public IEnumerable<Number> GetNumbers() => Left.GetNumbers().Concat(Right.GetNumbers());

    public IEnumerable<(Pair? pair, int level)> GetPairs(int level)
    {
        if (Left is Pair leftPair)
        {
            yield return (leftPair, level);
            foreach (var pair in leftPair.GetPairs(level + 1))
            {
                yield return pair;
            }
        }

        if (Right is Pair rightPair)
        {
            yield return (rightPair, level);
            foreach (var pair in rightPair.GetPairs(level + 1))
            {
                yield return pair;
            }
        }
    }
}

internal sealed class Number : IPart
{
    public Number(int value) => Value = value;

    public Pair? Parent { get; set; }

    public int Value { get; private set; }

    public void AddValue(int addition) => Value += addition;

    public int CalculateMagnitude() => Value;

    public IEnumerable<Number> GetNumbers()
    {
        yield return this;
    }
}