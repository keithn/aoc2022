namespace Aoc2022.days;

public class Day4
{
    record Range(int Start, int End);
    public static void Solve()
    {
            
        var lines = File.ReadLines("days/Day4.txt");
        var assignments = lines.Select(l => l.Split("," ).Select(ToRange).ToList()).ToList();
        
        Range ToRange(string s) => s.Split("-") is string[] p ? new Range(int.Parse(p[0]), int.Parse(p[1])) : null; 

        var covers = assignments .Count(r => Covers(r[0], r[1]) || Covers(r[1],r[0]));
        bool Covers(Range a, Range b) => a.Start<= b.Start&& a.End>= b.End;
        Console.WriteLine($"Part 1: {covers}");

        var overlaps = assignments.Count(r => Overlaps(r[0], r[1]) || Overlaps(r[1], r[0]));
        bool Overlaps(Range a, Range b) => (a.Start <= b.Start && a.End >= b.Start || (a.Start <= b.End && a.End >= b.End));
        Console.WriteLine($"Part 2: {overlaps}");
    }
}