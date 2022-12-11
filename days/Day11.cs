using System.Numerics;
using System.Text.RegularExpressions;

namespace Aoc2022.days;

public partial class Day11
{
    public record Monkey(List<BigInteger> Items, string Operation, BigInteger Test, int True, int False, BigInteger WorryFactor)
    {
        public BigInteger CalculateWorry(BigInteger item)
        {
            var eq = Operation.Split();
            var first = eq[0] == "old" ? item : BigInteger.Parse(eq[0]);
            var second = eq[2] == "old" ? item : BigInteger.Parse(eq[2]);
            return eq[1] switch
            {
                "+" => first + second,
                "*" => first * second,
                "/" => first / second,
                "-" => first - second,
            };
        }
        public BigInteger Inspections { get; set; } = 0;
    }

    public static void Solve()
    {
        var text = File.ReadAllLines("days/Day11.txt").Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        Dictionary<int, Monkey> ToMonkeys(List<string> text, int worryFactor)
        {
            var monkeys = new Dictionary<int, Monkey>();

            foreach (var chunk in text.Chunk(6))
            {
                var monkey = int.Parse(chunk.First().Split()[1].Replace(":", ""));
                var items = Numbers().Matches(chunk[1]).Select(m => BigInteger.Parse(m.Value)).ToList();
                var operation = chunk[2].Split("=")[1].Trim();
                var test = int.Parse(Numbers().Matches(chunk[3]).First().Value);
                var trueCase = int.Parse(Numbers().Matches(chunk[4]).First().Value);
                var falseCase = int.Parse(Numbers().Matches(chunk[5]).First().Value);
                monkeys.Add(monkey, new Monkey(items, operation, test, trueCase, falseCase, worryFactor));
            }

            return monkeys;
        }


        void MonkeyBusiness(Dictionary<int, Monkey> monkeys)
        {
            var monkeyOrder = monkeys.ToList().OrderBy(kv => kv.Key).Select(kv => kv.Value);
            // given all the test divisors are prime we can mod all the values by the product of all the divisors
            var productOfDivisors = monkeys.Select(kv => kv.Value.Test).Aggregate((a, b) => a * b);
            foreach (var monkey in monkeyOrder)
            {
                while (monkey.Items.Any())
                {
                    monkey.Inspections++;
                    var item = monkey.Items.First();
                    monkey.Items.RemoveAt(0);
                    var worry = BigInteger.Divide(monkey.CalculateWorry(item), monkey.WorryFactor) % productOfDivisors;

                    if ((worry % monkey.Test) == 0)
                    {
                        monkeys[monkey.True].Items.Add(worry);
                    }
                    else
                    {
                        monkeys[monkey.False].Items.Add(worry);
                    }
                }
            }
        }

        var monkeys = ToMonkeys(text, 3);
        Enumerable.Range(1, 20).ToList().ForEach(round => MonkeyBusiness(monkeys));
        var mostInspections = monkeys.ToList().Select(kv => kv.Value).OrderByDescending(m => m.Inspections).Take(2).Select(m => m.Inspections)
            .ToList();
        Console.WriteLine($"Part 1: {mostInspections[0] * mostInspections[1]}");

        monkeys = ToMonkeys(text, 1);
        Enumerable.Range(1, 10000).ToList().ForEach(round =>
        {
            MonkeyBusiness(monkeys);
        });
        mostInspections = monkeys.ToList().Select(kv => kv.Value).OrderByDescending(m => m.Inspections).Take(2).Select(m => m.Inspections).ToList();
        Console.WriteLine($"Part 2: {mostInspections[0] * mostInspections[1]}");
    }


    [GeneratedRegex("\\d+")]
    private static partial Regex Numbers();
}