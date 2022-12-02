using System.Text.RegularExpressions;

namespace Aoc2022.days;

public class Day1
{
    public static void Solve()
    {
        var elves = File.ReadAllText("days/Day1.txt")
            .Split("\r\n\r\n")
            .Select(elf => elf.Split("\r\n").Select(int.Parse));

        Console.WriteLine($"Part 1: {elves.Select(elf => elf.Sum()).Max()}");
        Console.WriteLine($"Part 2: {elves.Select(elf => elf.Sum()).OrderDescending().Take(3).Sum()}");
    }
}