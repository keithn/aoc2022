namespace Aoc2022.days;

public class Day8
{
    
    public static void Solve()
    {
        var lines = File.ReadAllLines("days/Day8.txt").Select(l => l.Trim()).ToList();
        var width = lines.First().Length;
        var height = lines.Count;

        Part1();
        Part2();

        // kindda ugly, but effective.
        void Part1()
        {
            int visible = width * 2 + height * 2 - 4;
            for (int row = 1; row < height - 1; row++)
            {
                for (int col = 1; col < width - 1; col++)
                {
                    var v = false;
                    var c = lines[row][col];
                    v |= lines[row][0..col].All(x => x < c);
                    v |= lines[row][(col+1)..(width)].All(x => x < c);
                    for (var t = row - 1; t >= 0 && lines[t][col] < c && !v; t--) if (t == 0) v = true;
                    for (var t = row + 1; t <= height - 1 && lines[t][col] < c && !v; t++) if (t == height - 1) v = true;
                    if (v) visible++;
                }
            }
            Console.WriteLine($"Part 1: {visible}");
        }
        // again ugly, but effective.
        void Part2()
        {
            int max = 0;
            for (int row = 1; row < height - 1; row++)
            {
                for (int col = 1; col < width - 1; col++)
                {
                    int sl=1, sr=1, sb=1, st=1;
                    var c = lines[row][col];
                    for (var l = col - 1; l > 0 && lines[row][l] < c; l--) sl++;
                    for (var l = col + 1; l < width - 1 && lines[row][l] < c ; l++) sr++;
                    for (var t = row - 1; t > 0 && lines[t][col] < c; t--) sb++;
                    for (var t = row + 1; t < height - 1 && lines[t][col] < c; t++) st++;
                    var score = sl* sr * sb * st;
                    if (score > max) max = score;
                }
            }
            Console.WriteLine($"Part 2: {max}");
        }
    }
}