using System.Buffers.Text;
using System.Text;
using System.Text.Json;

namespace Aoc2022.days;

public class Day10
{
    public static void Solve()
    {
        var tokens = File.ReadAllText("days/Day10.txt").Split(new[] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int x = 1;
        var snapshots = new List<int>();
        var sb = new StringBuilder();
        for (int i = 0; i < tokens.Length; i++)
        {
            var pixel = (i % 40);
            var lit = pixel >= (x - 1) && pixel <= (x + 1) ? "#" : " ";
            if (i % 40 == 0) sb.AppendLine();
            sb.Append(lit);
            if (new[] { 20, 60, 100, 140, 180, 220 }.Contains(i + 1))
            {
                snapshots.Add((i + 1) * x);
            }

            if (int.TryParse(tokens[i], out int v))
            {
                x += v;
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Part 1: {snapshots.Sum()}");
        // https://ocr.space/ocrapi
        var part2 = "see below";
        try
        {
            TextToImage.From(sb.ToString().Split("\r\n").ToList(), "day10.png");
            part2 = OCRSpace.Decode("day10.png");
        }
        catch
        {
        }
        Console.WriteLine($"Part 2: {part2}");
        Console.WriteLine(sb);
        
    }
}