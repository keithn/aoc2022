using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Aoc2022.days;

public partial class Day15
{
    readonly record struct Point(long X, long Y)
    {
        public static Point From(long[] n) => new(n[0], n[1]);
        public long DistanceTo(Point other) => long.Abs(X - other.X) + long.Abs(Y - other.Y);
    }

    record struct SensorBeacon(Point Sensor, Point Beacon, long Distance)
    {
        public bool CutsThroughY(long y) => y < Sensor.Y + Distance && y > Sensor.Y - Distance;
    };

    public static void Solve()
    {
        SensorBeacon ToSensorBeacon(List<long> sensorBeacons) 
        {
            var p1 = Point.From(sensorBeacons.Take(2).ToArray());
            var p2 = Point.From(sensorBeacons.Skip(2).Take(2).ToArray());
            return new SensorBeacon(p1, p2, p1.DistanceTo(p2));
        }

        var lines = File.ReadAllLines("days/Day15.txt").ToList();
        var devices = lines.Select(l => { return ToSensorBeacon(Numbers().Matches(l).Select(m => long.Parse(m.Value)).ToList()); }).ToList();

        var upperLimit = 4_000_000;

        long CountKnownFreeBounded(List<SensorBeacon> devices, long y)
        {
            var overlaps = new Stack<(long, long)>();
            var pc = 0;
            var inRange = devices.Where(d => d.CutsThroughY(y)).ToList();
            var beaconsOnY = devices.Select(p => p.Beacon).Distinct().Count(beacon => beacon.Y == y);
            var sensorsOnY = devices.Select(p => p.Sensor).Count(sensor => sensor.Y == y);
            var intervals = inRange.Select(sb =>
            {
                var count = sb.Distance - long.Abs(y - sb.Sensor.Y);
                var start = sb.Sensor.X - count;
                var addCount = count * 2 + 1;
                return (start, addCount);
            }).OrderBy(x => x.Item1).ToList();
            var sizes = intervals.Select(i =>
            {
                if (overlaps.Count == 0)
                {
                    overlaps.Push(i);
                    return i.Item2;
                }
                var top = overlaps.Peek();
                var endTop = top.Item1 + top.Item2;
                var endCurrent = i.Item1 + i.Item2;
                if (endTop > endCurrent) return 0; // this is a smaller than one of the previous
                overlaps.Pop();
                overlaps.Push(i);
                if (endTop >= i.Item1)
                {
                    var amount = i.Item2 - (endTop - i.Item1);
                    return amount;
                }
                return i.Item2;
            }).ToList();
            var sum = sizes.Sum();
            return sum - beaconsOnY - sensorsOnY;
        }

        BigInteger Part2(List<SensorBeacon> sensorBeacons)
        {
            foreach (var pair in sensorBeacons)
            {
                var y = 0;
                for (var x = long.Max(0, pair.Sensor.X - pair.Distance - 1); x < long.Max(upperLimit, pair.Sensor.X + pair.Distance + 1); x++)
                {
                    var points = new []{new Point(x, y+pair.Sensor.Y), new Point(x, pair.Sensor.Y-y)};
                    foreach (var point in points)
                    {
                        if (point.Y > upperLimit || point.Y < 0) continue;
                        if (sensorBeacons.All(sb => sb.Sensor.DistanceTo(point) > sb.Distance))
                        {
                            return new BigInteger(x) * new BigInteger(upperLimit) + point.Y;
                        }
                    }
                    y += (x > pair.Sensor.X) ? -1 : 1;
                }
            }
            return 0; // didn't find it.... 
        }

        Console.WriteLine($"Part 1: {CountKnownFreeBounded(devices, 2000000)}");
        var sw = Stopwatch.StartNew();
        Console.WriteLine($"Part 2: {Part2(devices)}");
        Console.WriteLine($"{sw.ElapsedMilliseconds/1000M}");

    }
    [GeneratedRegex("-?\\d+")]
    private static partial Regex Numbers();
}