var lines = File.ReadLines("input.txt");

Dictionary<char, int> scoreMap = new()
{
    { ')', 3 },
    { ']', 57 },
    { '}', 1197 },
    { '>', 25137 }
};

Dictionary<char, char> charMap = new()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' }
};

int score = 0;

foreach (string line in lines)
{
    Stack<char> stack = new();
    foreach(char character in line)
    {
        if(IsOpeningCharacter(character))
        {
            stack.Push(character);
        }

        if(IsClosingCharacter(character))
        {
            if(!stack.TryPop(out char lastOpened) ||
               charMap[lastOpened] != character)
            {
                score += scoreMap[character];
                break;
            }
        }
    }
}

Console.WriteLine(score);

bool IsOpeningCharacter(char c) => charMap.ContainsKey(c);

bool IsClosingCharacter(char c) => charMap.ContainsValue(c);