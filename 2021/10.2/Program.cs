var lines = File.ReadLines("input.txt");

Dictionary<char, int> scoreMap = new()
{
    { ')', 1 },
    { ']', 2 },
    { '}', 3 },
    { '>', 4 }
};

Dictionary<char, char> charMap = new()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' }
};

List<long> scores = new();

foreach (string line in lines)
{
    Stack<char> stack = new();
    bool isCorrupt = false;
    foreach(char character in line)
    {
        if(IsOpeningCharacter(character))
        {
            stack.Push(character);
        }

        if(IsClosingCharacter(character))
        {
            if(!stack.TryPop(out char lastOpeningChar) ||
               charMap[lastOpeningChar] != character)
            {
                isCorrupt = true;
                break;
            }
        }
    }

    if(isCorrupt)
    {
        continue;
    }

    bool isIncomplete = stack.Any();
    if(isIncomplete)
    {
        long scoreForThisLine = 0;
        while (stack.TryPop(out char lastOpeningChar))
        {
            char closingChar = charMap[lastOpeningChar];
            scoreForThisLine *= 5;
            scoreForThisLine += scoreMap[closingChar];
        }

        scores.Add(scoreForThisLine);
    }
}

scores.Sort();
Console.WriteLine(scores[scores.Count / 2]);

bool IsOpeningCharacter(char c) => charMap.ContainsKey(c);

bool IsClosingCharacter(char c) => charMap.ContainsValue(c);