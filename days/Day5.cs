namespace Aoc2022.days;

public class Day5
{
    record Instruction(int From, int To, int Count);

    public static void Solve()
    {
        var lines = File.ReadLines("days/Day5.txt");

        List<Stack<char>> ParseStacksDefinition(IEnumerable<string> lines)
        {
            var stackDefinition = lines.TakeWhile(l => !l.Contains("move"))
                .Where(l => !string.IsNullOrWhiteSpace(l)) 
                .Reverse()  // reverse so the first things to push to the stack are at the top
                .Skip(1)    // skip the stack numbering line
                .Select(l => l.Chunk(4).Select(c => c.Where(char.IsLetter).FirstOrDefault()).ToList()).ToList();
            return Enumerable.Range(0, stackDefinition.First().Count())
                .Select(stack =>
                    new Stack<char>(Enumerable.Range(0, stackDefinition.Count).Select(height => stackDefinition[height][stack]).Where(char.IsLetter)))
                .ToList();
        }

        Instruction ParseInstruction(string description) =>
            description.Split(new[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() is int[] v
                ? new Instruction(v[1], v[2], v[0])
                : null;

        var instructions = lines.SkipWhile(l => !l.Contains("move")).Select(ParseInstruction).ToList();

        var stacks = ParseStacksDefinition(lines);
        foreach (var instruction in instructions)
        {
            Enumerable.Range(0, instruction.Count).ToList()
                .ForEach(_ => stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop()));
        }

        Console.WriteLine($"Part 1: {new string(stacks.Select(s => s.Peek()).ToArray())}");

        stacks = ParseStacksDefinition(lines);
        foreach (var instruction in instructions)
        {
            Enumerable.Range(0, instruction.Count)
                .Select(_ => stacks[instruction.From - 1].Pop())
                .Reverse().ToList()
                .ForEach(v => stacks[instruction.To - 1].Push(v));
        }

        Console.WriteLine($"Part 2: {new string(stacks.Select(s => s.Peek()).ToArray())}");
    }
}