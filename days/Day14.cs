using System.Text;

namespace Aoc2022.days;

public class Day14
{
    public enum Soil
    {
        Rock,
        Sand,
        Empty
    }
    public readonly record struct Point(int X, int Y)
    {
        public static Point From(int[] n) => new(n[0], n[1]);
        public Point Add((int x, int y) change) => new(X + change.x, Y + change.y);
    }

    public static void Solve()
    {
        var rockScans = File.ReadLines("days/Day14.txt").Select(ToPoints).ToList();
        List<Point> ToPoints(string l) => l.Split("->", StringSplitOptions.TrimEntries).Select(ToPoint).ToList();
        Point ToPoint(string p) => Point.From(p.Split(',').Select(int.Parse).ToArray());
        int Clamp(int x) => int.Clamp(x, -1, 1);

        Dictionary<Point, Soil> PlaceRocks(Dictionary<Point, Soil> cave, List<List<Point>> rockScans)
        {
            foreach (var scan in rockScans)
            {
                var current = scan.First();
                foreach (var target in scan.Skip(1))
                {
                    cave.TryAdd(current, Soil.Rock);
                    var change = (x: Clamp(target.X - current.X), y: Clamp(target.Y - current.Y));
                    while (current != target)
                    {
                        current = current.Add(change);
                        cave.TryAdd(current, Soil.Rock);
                    }
                }
            }
            return cave;
        }
        Dictionary<Point, Soil> SimulateSand(Dictionary<Point, Soil> cave, bool exitBeforeFloor = true)
        {
            var drops = new[] { (0, 1), (-1, 1), (1, 1) };
            var bottom = cave.Select(p => p.Key.Y).Max();
            var origin = new Point(500, 0);
            while (true)
            {
                var unit = origin;
                while (unit.Y < bottom || !exitBeforeFloor)
                {
                    var candidates = drops.Select(d => unit.Add(d)).ToList();
                    var hit = candidates.Where(p => cave.ContainsKey(p) || p.Y == bottom+2).ToList();
                    if (hit.Count == drops.Length)
                    {
                        cave.Add(unit, Soil.Sand);
                        break;
                    }
                    unit = candidates.First(p => !cave.ContainsKey(p));
                }
                if ((unit.Y >= bottom && exitBeforeFloor) || cave.ContainsKey(origin)) return cave; 
            }
        }
        var cave = SimulateSand(PlaceRocks(new Dictionary<Point, Soil>(), rockScans));
        Console.WriteLine($"Part 1: {cave.Values.Count(v => v == Soil.Sand)}");
        
        var cave2 = SimulateSand(PlaceRocks(new Dictionary<Point, Soil>(), rockScans), false);
        Console.WriteLine($"Part 2: {cave2.Values.Count(v => v == Soil.Sand)}");
        
        ////////// Everything Below is just fun and to produce a render... ///////////
        Render(cave, "day14-part1.png");
        Render(cave2, "day14-part2.png");
    }

    public static void Render(Dictionary<Point, Soil> cave, string fileName)
    {
        var sb = new StringBuilder();
        var xx = cave.Select(p => p.Key.X).Min();
        var yy = cave.Select(p => p.Key.Y).Min();
        var xX = cave.Select(p => p.Key.X).Max();
        var yY = cave.Select(p => p.Key.Y).Max();

        for (int y = yy; y <= yY; y++)
        {
            for (int x = xx; x <= xX; x++)
            {
                var t = (cave.TryGetValue(new Point(x, y), out var soil) ? soil : Soil.Empty) switch
                {
                    Soil.Rock => '#',
                    Soil.Sand => 'o',
                    _ => '.'
                };
                sb.Append(t);
            }
            sb.AppendLine();
        }
        TextToImage.From(sb.ToString().Split("\r\n").ToList(), fileName, SandRockColorMap);
    }

    private static readonly Dictionary<char?, string> SandRockColorMap = new()
    {
        { '#', "5A4D41" },
        { 'o', "C2B280" },
        { '.', "FFFFFF" },
    };
}