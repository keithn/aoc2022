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
            var p = Last;
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
            char NoMarker(char c) => c switch { 'S' => 'a', 'E' => 'z', _ => c };
            if (to.X < 0 || to.X >= terrain[0].Length || to.Y < 0 || to.Y >= terrain.Length || terrain[from.Y][from.X].Visited) return false;
            return rule(
                NoMarker(terrain[from.Y][from.X].Height),
                NoMarker(terrain[to.Y][to.X].Height)
            );
        }
        WayPoint Navigate(Location[][] terrain, char beginAt, char End, Func<char, char, bool> rule)
        {
            var start = terrain.SelectMany((l, y) => l.Select((h, x) => new WayPoint(h.Height == beginAt ? x : -1, y))).First(p => p.X > -1);
            Queue<WayPoint> queue = new(new []{start});
            int i = 0;
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
                i++;
            }
            return null; // no way!
        }

        var path = Navigate(ToTerrain(lines), 'S', 'E', (f, t) => f - t >= -1);
        Console.WriteLine($"Part 1: {path.Seen().Count() }");

        var path2 = Navigate(ToTerrain(lines), 'E', 'a', (f, t) => f - t <= 1);
        Console.WriteLine($"Part 2: {path2.Seen().Count() }");
        
        Render(lines.ToList(), path, "day12-part1.png");
        Render(lines.ToList(), path2, "day12-part2.png");
    }

    private static void Render(List<string> lines, WayPoint wayPoint, string filename = "Day12.png")
    {
        foreach (var point in wayPoint.Seen())
        {
            var line = lines[point.Y].ToArray();
            line[point.X] = 'P';
            lines[point.Y] = new string(line);
        }
        TextToImage.From(lines, filename, TerrainColorMap);
    }
    private static readonly Dictionary<char?, string> TerrainColorMap = new()
    {
        { '#', "4E892A" },
        { 'a', "fafa6e" },
        { 'b', "e6f56f" },
        { 'c', "d3f072" },
        { 'd', "c0eb75" },
        { 'e', "ade578" },
        { 'f', "9cdf7c" },
        { 'g', "8ad97f" },
        { 'h', "7ad283" },
        { 'i', "69cb86" },
        { 'j', "5ac489" },
        { 'k', "4abd8c" },
        { 'l', "3bb58e" },
        { 'm', "2bad8f" },
        { 'n', "1aa68f" },
        { 'o', "059e8f" },
        { 'p', "00968e" },
        { 'q', "008e8c" },
        { 'r', "008689" },
        { 's', "007e86" },
        { 't', "027681" },
        { 'u', "106e7c" },
        { 'v', "196676" },
        { 'w', "1f5e6f" },
        { 'x', "245768" },
        { 'y', "284f60" },
        { 'z', "2a4858" },
        { 'S', "FF0000" },
        { 'P', "AAAA0000" },
        { 'E', "AAAA0000" },
    };
}