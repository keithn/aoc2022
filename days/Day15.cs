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

    record struct SensorBeacon(Point Sensor, Point Beacon)
    {
        public long Distance = Sensor.DistanceTo(Beacon);
        public bool InRange(Point p) => Sensor.DistanceTo(p) <= Distance;
        public bool CutsThroughY(long y) => y < Sensor.Y + Distance && y > Sensor.Y - Distance;
    };

    public static void Solve()
    {
        SensorBeacon ToSensorBeacon(List<long> sensorBeacons) =>
            new(Point.From(sensorBeacons.Take(2).ToArray()), Point.From(sensorBeacons.Skip(2).Take(2).ToArray()));

        var lines = File.ReadAllLines("days/Day15.txt").ToList();
        var devices = lines.Select(l => { return ToSensorBeacon(Numbers().Matches(l).Select(m => long.Parse(m.Value)).ToList()); }).ToList();

        var upperLimit = 4_000_000;

        (long, List<(long, long)> intervals) CountKnownFreeBounded(List<SensorBeacon> devices, long y, bool bounded)
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
                if (bounded)
                {
                    if (start < 0)
                    {
                        var diff = long.Abs(start);
                        if (addCount - diff < 0) return (0, 0);
                        return (0, addCount - diff);
                    }
                    if (start + addCount > upperLimit)
                    {
                        addCount = upperLimit - start;
                        if (addCount <= 0) return (0, 0);
                    }
                }
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
            return bounded?(sum,intervals) : (sum - beaconsOnY - sensorsOnY, intervals);
        }

        var (c, i) = CountKnownFreeBounded(devices, 11, true);
        
        Console.WriteLine($"Part 1: {CountKnownFreeBounded(devices, 2000000, false)}");

        BigInteger part2 = 0;
        for (var y = 0; y <= upperLimit; y++)
        {
            var (count, intervals) = CountKnownFreeBounded(devices, y, true);
            if (count != upperLimit)
            {
                for (var x = 0; x <= upperLimit; x++)
                {
                    if (!intervals.Any(interval => x >= interval.Item1 && x < interval.Item1+interval.Item2))
                    {
                        part2 = new BigInteger(x) * new BigInteger(upperLimit) + y;
                        break;
                    }
                }
                break;
            }
        }
        Console.WriteLine($"Part 2: {part2}");
    }
    [GeneratedRegex("-?\\d+")]
    private static partial Regex Numbers();
}