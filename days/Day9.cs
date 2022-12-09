namespace Aoc2022.days;

public class Day9
{

    public record struct Point(int X, int Y)
    {
        public Point Move((int dx, int dy) direction) => new(X + direction.dx, Y + direction.dy);
        public Point Follow(Point head)
        {
            var y = head.Y - Y;
            var x = head.X - X;
            if (int.Abs(y) <= 1 && int.Abs(x) <= 1) return this;
            return new Point(X + int.Clamp(x,-1,1), Y + int.Clamp(y,-1,1));
        }
    }

    public static void Solve()
    {
        var lines = File.ReadLines("days/Day9.txt");
        var trail = TailTrail(lines, Enumerable.Range(1, 2).Select(_ => new Point(0, 0)).ToList());
        Console.WriteLine($"Part 1: {trail.Distinct().Count()}");
        trail = TailTrail(lines, Enumerable.Range(1, 10).Select(_ => new Point(0, 0)).ToList());
        Console.WriteLine($"Part 2: {trail.Distinct().Count()}");
    }

    private static List<Point> TailTrail(IEnumerable<string> lines, List<Point> rope)
    {
        var trail = new List<Point>();
        foreach (var line in lines)
        {
            var movement = line.Split();
            var direction = movement[0] switch
            {
                "R" => (1, 0),
                "L" => (-1, 0),
                "U" => (0, 1),
                "D" => (0, -1),
            };
            var count = int.Parse(movement[1]);
            for (int _ = 0; _ < count; _++)
            {
                rope[0] = rope[0].Move(direction);
                for(var k = 1; k<rope.Count;k++)
                {
                    rope[k] = rope[k].Follow(rope[k-1]);
                    if(k == rope.Count-1) trail.Add(rope[k]);
                }
            }
        }
        return trail;
    }
}