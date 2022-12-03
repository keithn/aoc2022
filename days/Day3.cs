namespace Aoc2022.days;

public class Day3
{
    public static void Solve()
    {
        int Score(char letter) => char.IsLower(letter) ? letter - 'a' + 1 : letter - 'A' + 27;

        var lines = File.ReadLines("days/Day3.txt");

        var sum = lines.Select(l => (first: l[0..(l.Length / 2)], second: l[(l.Length / 2)..]))
            .Select(l => l.first.Intersect(l.second).First())
            .Select(Score)
            .Sum();


        Console.WriteLine($"Part 1: {sum}");

        sum = lines.Select((s, i) => (idx: i / 3, v: s)).GroupBy(x => x.idx)
            .Select(g => g.Select(kv => kv.v).ToList())
            .Select(g => g[0].Intersect(g[1].Intersect(g[2])).First())
            .Select(Score)
            .Sum();
        
        Console.WriteLine($"Part 2: {sum}");
    }
}