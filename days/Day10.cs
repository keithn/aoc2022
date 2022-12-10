namespace Aoc2022.days;

public class Day10
{
    public static void Solve()
    {
        var tokens = File.ReadAllText("days/Day10.txt").Split(new[] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int x = 1;
        var snapshots = new List<int>();
        for (int i = 0; i < tokens.Length; i++)
        {
            var pixel = (i % 40);
            var lit = pixel>= (x - 1) && pixel <= (x + 1) ? "#" : ".";
            if(i%40==0) Console.WriteLine();
            Console.Write(lit);
            if (new []{20, 60, 100, 140, 180, 220}.Contains(i+1))
            {
                snapshots.Add((i+1) * x);
            }
            if (int.TryParse(tokens[i], out int v))
            {
                x += v;
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Part 1: {snapshots.Sum()}");
        Console.WriteLine($"Part 2: see print out");
    }
}