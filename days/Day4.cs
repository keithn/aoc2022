namespace Aoc2022.days;

public class Day4
{
    record Range(int Start, int End);
    public static void Solve()
    {
            
        var lines = File.ReadLines("days/Day4.txt");
        var assignments = lines.Select(l => l.Split("," ).Select(ToRange).ToList()).ToList();
        
        Range ToRange(string s)
        {
            var parts = s.Split("-");
            return new Range(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        var covers = assignments .Count(r => Covers(r[0], r[1]) || Covers(r[1],r[0]));
        bool Covers(Range a, Range b) => a.Start<= b.Start&& a.End>= b.End;
        Console.WriteLine($"Part 1: {covers}");

        var overlaps = assignments.Count(r => Overlaps(r[0], r[1]) || Overlaps(r[1], r[0]));
        bool Overlaps(Range a, Range b) => (a.Start <= b.Start && a.End >= b.Start || (a.Start <= b.End && a.End >= b.End));
        Console.WriteLine($"Part 2: {overlaps}");
    }
}