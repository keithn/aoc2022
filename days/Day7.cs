namespace Aoc2022.days;

public class Day7
{
    public abstract record FileSystem
    {
        public virtual int Size { get; init; }

        public record Folder(string Name, Folder Parent) : FileSystem
        {
            public List<FileSystem> Entries = new();
            public override int Size => Entries.Sum(e => e.Size);
        };
        public record File(string Name) : FileSystem
        {
            public override int Size { get; init; }
        };
        public static IEnumerable<FileSystem> Items(FileSystem item)
        {
            if (item is Folder folder)
            {
                foreach (var entry in folder.Entries)
                {
                    yield return entry;
                    foreach (var subItem in Items(entry)) yield return subItem;
                }
            }
            else yield return item;
        }
    }
    public static void Solve()
    {
        var lines = File.ReadLines("days/Day7.txt");

        FileSystem.Folder Parse(IEnumerable<string> lines)
        {
            var filesystem = new FileSystem.Folder("", null);
            var current = filesystem;
            foreach (var line in lines)
            {
                if (line.StartsWith("$"))
                {
                    var parts = line.Replace("$ ", "").Split(" ", StringSplitOptions.TrimEntries);
                    current = parts[0] switch
                    {
                        "ls" => current,
                        "cd" => parts[1] switch
                        {
                            ".." => current.Parent,
                            "/" => filesystem,
                            (var dir) => current.Entries.OfType<FileSystem.Folder>().FirstOrDefault(f => f.Name == dir)
                        }
                    };
                }
                else
                {
                    var parts = line.Replace("$", "").Split(" ", StringSplitOptions.TrimEntries);
                    current.Entries.Add((int.TryParse(parts[0], out int size))
                        ? (new FileSystem.File(parts[1]) { Size = size })
                        : (new FileSystem.Folder(parts[1], current )));
                }
            }
            return filesystem;
        }
        var filesystem = Parse(lines);
        var part1= FileSystem.Items(filesystem).OfType<FileSystem.Folder>().Where(f => f.Size <= 100000).Sum(f => f.Size);
        Console.WriteLine($"Part 1: {part1}");

        var totalSize = filesystem.Size;
        var part2= FileSystem.Items(filesystem).OfType<FileSystem.Folder>().Where(f => (70000000-totalSize) + f.Size >= 30000000).Min(f => f.Size);
        Console.WriteLine($"Part 2: {part2}");
    }
}