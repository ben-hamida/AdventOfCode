var disk = File.ReadAllText("input.txt")
    .Select(c => int.Parse(c.ToString()))
    .Select<int, DiskMemory>((number, index) => index % 2 == 0
        ? new FileMemory(number, index / 2)
        : new FreeMemory(number))
    .ToArray();

var compactedMemory = disk.ToList();
foreach (var file in disk.OfType<FileMemory>().Reverse())
{
    foreach (var freeMemory in compactedMemory.TakeWhile(m => m != file).OfType<FreeMemory>())
    {
        if (freeMemory.Size >= file.Size)
        {
            var freeMemoryIndex = compactedMemory.IndexOf(freeMemory);
            compactedMemory[compactedMemory.IndexOf(file)] = new FreeMemory(file.Size);
            compactedMemory[freeMemoryIndex] = new FreeMemory(freeMemory.Size - file.Size);
            compactedMemory.Insert(freeMemoryIndex, file);
            break;
        }
    }
}

var checksum = compactedMemory
    .SelectMany(m => Enumerable.Repeat((m as FileMemory)?.Id ?? 0, m.Size))
    .Index()
    .Sum(x => x.Index * (double)x.Item);

Console.WriteLine(checksum);

internal class DiskMemory(int size)
{
    public int Size { get; } = size;
}

internal class FreeMemory(int size) : DiskMemory(size);

internal class FileMemory(int size, int id) : DiskMemory(size)
{
    public int Id => id;
}