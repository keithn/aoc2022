namespace Aoc2022.days;

public class Day12
{
    public record Height(char C)
    {
        public bool Visited { get; set; } = false;
    }

    public record Point(int X, int Y)
    {
        public Point? Last { get; set; } = null;
        public Point Up => new(X, Y - 1);
        public Point Down => new(X, Y + 1);
        public Point Left => new(X - 1, Y);
        public Point Right => new(X + 1, Y);
        public IEnumerable<Point> Seen()
        {
            var p = this;
            while (p != null)
            {
                yield return p;
                p = p.Last;
            }
        }
    };

    public static void Solve()
    {
        var lines = File.ReadLines("days/Day12.txt").ToList();
        Height[][]? heightMap = FromLines(lines);
        int width = heightMap.First().Length;
        int height = heightMap.Length;

        Height[][]? FromLines(List<string> lines) => lines.Select(l => l.Select(x => new Height(x)).ToArray()).ToArray();
        bool Traversable(Point from, Point to)
        {
            if (to.X < 0 || to.X >= width || to.Y < 0 || to.Y >= height || heightMap[from.Y][from.X].Visited) return false;
            var f = heightMap[from.Y][from.X].C;
            var t = heightMap[to.Y][to.X].C;
            if (f == 'S') f = 'a';
            if (t == 'E') t = 'z';
            return (f - t >= -1);
        }
        Point FindEnd(Point Start)
        {
            Queue<Point> queue = new();
            queue.Enqueue(Start);
            while (queue.Any())
            {
                var p = queue.Dequeue();
                if (heightMap[p.Y][p.X].C == 'E') return p;
                new[] { p.Up, p.Down, p.Left, p.Right }.Where(n => Traversable(p, n)).ToList().ForEach(n =>
                {
                    n.Last = p;
                    queue.Enqueue(n);
                });
                heightMap[p.Y][p.X].Visited = true;
            }
            return null; // no way!
        }

        void Reset(Height[][] heights)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++) heights[y][x].Visited = false;
            }
        }
        var S = heightMap.ToArray().SelectMany((l, y) => l.Select((h, x) => new Point(h.C == 'S' ? x : -1, y))).First(p => p.X > -1);
        var path = FindEnd(S);
        var route = path.Seen().ToList();
        Console.WriteLine($"Part 1: {route.Count() - 1}");


        var lengths = new List<int>();
        var startingPoints = heightMap.ToArray().SelectMany((l, y) => l.Select((h, x) => new Point(h.C == 'S' || h.C =='a' ? x : -1, y))).Where(p => p.X > -1).ToList();
        foreach (var point in startingPoints)
        {
            Reset(heightMap);
            var maybePath = FindEnd(point);
            if (maybePath is not null)
            {
                lengths.Add(maybePath.Seen().Count()-1);
            }
        }
        Console.WriteLine($"Part 2: {lengths.Min()}");
    }
}