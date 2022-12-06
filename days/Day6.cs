namespace Aoc2022.days;

public class Day6
{
    public static void Solve()
    {
        var text = File.ReadAllText("days/Day6.txt");
        int StartOf(string text, int n)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (new HashSet<char>(text.Skip(i).Take(n)).Count == n) return i + n;
            }
            return 0;
        }
        Console.WriteLine($"Part 1: {StartOf(text, 4)}");
        Console.WriteLine($"Part 2: {StartOf(text, 14)}");
    }
}