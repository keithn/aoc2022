using System.Data;

namespace Aoc2022.days;

public class Day12
{
    private record Location(char Height)
    {
        public bool Visited { get; set; } = false;
    }

    public record WayPoint(int X, int Y)
    {
        public WayPoint? Last { get; set; } = null;
        public WayPoint Up => new(X, Y - 1);
        public WayPoint Down => new(X, Y + 1);
        public WayPoint Left => new(X - 1, Y);
        public WayPoint Right => new(X + 1, Y);

        public IEnumerable<WayPoint> Seen()
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
        Location[][] ToTerrain(IEnumerable<string> list) => list.Select(l => l.Select(x => new Location(x)).ToArray()).ToArray();
        
        bool Traversable(Location[][] terrain, WayPoint from, WayPoint to, Func<char, char, bool> rule)
        {
            char Etoz(char c) => c == 'E' ? 'z' : c;
            char SEtoaz(char c) => c == 'S' ? 'a' : Etoz(c);
            if (to.X < 0 || to.X >= terrain[0].Length || to.Y < 0 || to.Y >= terrain.Length || terrain[from.Y][from.X].Visited) return false;
            return rule(
                SEtoaz(terrain[from.Y][from.X].Height),
                SEtoaz(terrain[to.Y][to.X].Height)
            );
        }
        WayPoint FindEnd(Location[][] terrain, char beginAt, char End, Func<char, char, bool> rule)
        {
            var start = terrain.SelectMany((l, y) => l.Select((h, x) => new WayPoint(h.Height == beginAt ? x : -1, y))).First(p => p.X > -1);
            Queue<WayPoint> queue = new();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var p = queue.Dequeue();
                if (terrain[p.Y][p.X].Height == End) return p;
                new[] { p.Up, p.Down, p.Left, p.Right }.Where(n => Traversable(terrain, p, n, rule)).ToList().ForEach(n =>
                {
                    n.Last = p;
                    queue.Enqueue(n);
                });
                terrain[p.Y][p.X].Visited = true;
            }
            return null; // no way!
        }

        var path = FindEnd(ToTerrain(lines), 'S', 'E', (f, t) => f - t >= -1);
        Console.WriteLine($"Part 1: {path.Seen().Count() - 1}");

        var path2 = FindEnd(ToTerrain(lines), 'E', 'a', (f, t) => f - t <= 1);
        Console.WriteLine($"Part 2: {path2.Seen().Count() - 1}");
    }
}