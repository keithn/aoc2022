using System.Text;

namespace Aoc2022.days;

public class Day14
{
    public enum Soil { Rock, Sand, Empty }
    public class Cave : Dictionary<Point, Soil>
    {
        int Clamp(int x) => int.Clamp(x, -1, 1);
        Point DrawPoint(Point p, Soil soil) =>  TryAdd(p, soil)?p:p;
        Point DrawLine(Point from, Point to)
        {
            var delta = (x: Clamp(to.X - from.X), y: Clamp(to.Y - from.Y));
            DrawPoint(from, Soil.Rock);
            while (from != to) from = DrawPoint(from.Add(delta), Soil.Rock); 
            return from;
        }
        public Cave PlaceRocks(List<List<Point>> rockScans)
        {
            rockScans.ForEach(scan => scan.Aggregate(DrawLine));
            return this;
        }
        private readonly (int, int)[] _dropDirections = { (0, 1), (-1, 1), (1, 1) };
        private Point DropSand(Point grainOfSand, int floor)
        {
            while (grainOfSand.Y < floor)
            {
                var candidates = _dropDirections.Select(d => grainOfSand.Add(d)).ToList();
                if (candidates.All(p => ContainsKey(p) || p.Y == floor)) return grainOfSand;
                grainOfSand = candidates.First(p => !ContainsKey(p));
            }
            return grainOfSand;
        }
        public Cave SimulateSand(bool exitBeforeFloor = true)
        {
            var floor = this.Select(p => p.Key.Y).Max() + 2;
            var sandEntryPoint = new Point(500, 0);
            while (true)
            {
                var grainOfSand = DropSand(sandEntryPoint, floor);
                if (exitBeforeFloor && grainOfSand.Y + 1 == floor) return this;
                if(DrawPoint(grainOfSand, Soil.Sand) == sandEntryPoint) return this;
            }
        }
    }
    public readonly record struct Point(int X, int Y)
    {
        public static Point From(int[] n) => new(n[0], n[1]);
        public Point Add((int x, int y) change) => new(X + change.x, Y + change.y);
    }

    public static void Solve()
    {
        List<Point> ToPoints(string l) => l.Split("->", StringSplitOptions.TrimEntries).Select(ToPoint).ToList();
        Point ToPoint(string p) => Point.From(p.Split(',').Select(int.Parse).ToArray());
        var rockScans = File.ReadLines("days/Day14.txt").Select(ToPoints).ToList();
        
        var cave = new Cave().PlaceRocks(rockScans).SimulateSand();
        Console.WriteLine($"Part 1: {cave.Values.Count(v => v == Soil.Sand)}");
        
        var cave2 = new Cave().PlaceRocks(rockScans).SimulateSand(false);
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