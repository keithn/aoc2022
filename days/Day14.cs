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
        List<List<Point>> rockScans = File.ReadLines("days/Day14.txt")
            .Select(ToPoints).ToList();

        List<Point> ToPoints(string l) =>
            l.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(ToPoint).ToList();

        Point ToPoint(string p) => Point.From(p.Split(',').Select(int.Parse).ToArray());

        int Clamp(int x) => int.Clamp(x, -1, 1);

        void PlaceRocks(Dictionary<Point, Soil> pit, List<List<Point>> rockScans)
        {
            foreach (var scan in rockScans)
            {
                var current = scan.First();
                foreach (var target in scan.Skip(1))
                {
                    pit.TryAdd(current, Soil.Rock);
                    var change = (x: Clamp(target.X - current.X), y: Clamp(target.Y - current.Y));
                    while (current != target)
                    {
                        current = current.Add(change);
                        pit.TryAdd(current, Soil.Rock);
                    }
                }
            }
        }

        void SimulateSand(Dictionary<Point, Soil> pit, bool exitBeforeFloor = true)
        {
            var drops = new[] { (0, 1), (-1, 1), (1, 1) };
            var bottom = pit.Select(p => p.Key.Y).Max();
            var origin = new Point(500, 0);
            while (true)
            {
                var unit = origin;
                while (unit.Y < bottom || !exitBeforeFloor)
                {
                    var candidates = drops.Select(d => unit.Add(d)).ToList();
                    var hit = candidates.Where(p => pit.ContainsKey(p) || p.Y == bottom+2).ToList();
                    if (hit.Count == drops.Length)
                    {
                        pit.Add(unit, Soil.Sand);
                        break;
                    }
                    unit = candidates.First(p => !pit.ContainsKey(p));
                }
                if ((unit.Y >= bottom && exitBeforeFloor) || pit.ContainsKey(origin)) return;
            }
        }
        var pit = new Dictionary<Point, Soil>();
        PlaceRocks(pit, rockScans);
        SimulateSand(pit);
        Render(pit, "day14-part1.png");

        Console.WriteLine($"Part 1: {pit.Values.Count(v => v == Soil.Sand)}");
        
        var pit2 = new Dictionary<Point, Soil>();
        PlaceRocks(pit2, rockScans);
        SimulateSand(pit2, false);
        Console.WriteLine($"Part 2: {pit2.Values.Count(v => v == Soil.Sand)}");
        Render(pit2, "day14-part2.png");
    }


    // Everything below is Rending a Picture For Fun!
    public static void Render(Dictionary<Point, Soil> pit, string fileName)
    {
        var sb = new StringBuilder();
        var xx = pit.Select(p => p.Key.X).Min();
        var yy = pit.Select(p => p.Key.Y).Min();
        var xX = pit.Select(p => p.Key.X).Max();
        var yY = pit.Select(p => p.Key.Y).Max();

        for (int y = yy; y <= yY; y++)
        {
            for (int x = xx; x <= xX; x++)
            {
                var t = (pit.TryGetValue(new Point(x, y), out var soil) ? soil : Soil.Empty) switch
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