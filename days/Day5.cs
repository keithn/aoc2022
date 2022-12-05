namespace Aoc2022.days;

public class Day5
{
    record Instruction(int From, int To, int Count);
    public static void Solve()
    {
        var lines = File.ReadLines("days/Day5.txt");

        List<Stack<char>> ParseDockDefinition(IEnumerable<string> lines)
        {
            var stackDefinition = lines.TakeWhile(l => !l.Contains("move")).Where(l => !string.IsNullOrWhiteSpace(l)).Reverse().ToList();
            var s = stackDefinition.Skip(1).Select(l => l.Chunk(4).Select(c => c.Where(char.IsLetter).FirstOrDefault()).ToList()).ToList();
            var dock = new List<Stack<char>>();
            for (int stack = 0; stack < s.First().Count(); stack++)
            {
                dock.Add(new Stack<char>());
                for (int height = 0; height < s.Count(); height++)
                {
                    if (char.IsLetter(s[height][stack])) dock[stack].Push(s[height][stack]);
                }
            }
            return dock;
        }
        Instruction ParseInstruction(string description) =>
            description.Split(new[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() is int[] v
                ? new Instruction(v[1], v[2], v[0])
                : null;

        var instructions = lines.SkipWhile(l => !l.Contains("move")).Select(ParseInstruction).ToList();
        
        var dock = ParseDockDefinition(lines);
        foreach (var instruction in instructions)
        {
            Enumerable.Range(0, instruction.Count).ToList()
                .ForEach(_ => dock[instruction.To-1].Push(dock[instruction.From-1].Pop()));
        }
        Console.WriteLine($"Part 1: {new string(dock.Select(s => s.Peek()).ToArray())}");
        
        dock = ParseDockDefinition(lines);
        foreach (var instruction in instructions)
        {
            Enumerable.Range(0, instruction.Count)
                .Select(_ => dock[instruction.From - 1].Pop())
                .Reverse().ToList()
                .ForEach(v => dock[instruction.To-1].Push(v));
        }
        Console.WriteLine($"Part 2: {new string(dock.Select(s => s.Peek()).ToArray())}");
    }
}