var disk = File.ReadAllText("input.txt")
    .Select(c => int.Parse(c.ToString()))
    .Select<int, Memory>((number, index) => index % 2 == 0
        ? new FileMemory(number, index / 2)
        : new FreeMemory(number))
    .SelectMany(memory => Enumerable.Repeat<int?>(memory is FileMemory fileMemory ? fileMemory.Id : null, memory.Size))
    .ToArray();

var fileMemoryCount = disk.Count(m => m is not null);

var compactedMemory = disk.ToList();
foreach (var id in disk.Reverse().Index())
{
    if (id.Item is not null)
    {
        var emptyMemoryIndex = compactedMemory.IndexOf(null);
        if(emptyMemoryIndex >= fileMemoryCount - 1)
        {
            break;
        }

        compactedMemory[emptyMemoryIndex] = id.Item;
        compactedMemory[compactedMemory.Count - id.Index - 1] = null;
    }
}

var checksum = compactedMemory.OfType<int>().Index().Sum(x => x.Index * (double)x.Item);

Console.WriteLine(checksum);

internal record Memory(int Size);
internal record FreeMemory(int Size) : Memory(Size);
internal record FileMemory(int Size, int Id) : Memory(Size);