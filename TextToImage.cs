namespace Aoc2022;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class TextToImage
{
    public static void From(List<string> lines, string fileName, Dictionary<char?, string> colorMap = null)
    {
        if (colorMap == null) colorMap = DefaultMap;
        var width = lines.Max(l => l.Length) * 10;
        var height = lines.Count * 5 + 10;
        var bitmap = new Bitmap(width, height);
        using var g = Graphics.FromImage(bitmap);
        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                var color = colorMap.ContainsKey(c) ? colorMap[c] : "00000000";
                if (color.Length == 6) color = "FF" + color;
                using var brush = new SolidBrush(Color.FromArgb(Convert.ToInt32(color, 16)));
                g.FillRectangle(brush, 5 + (x * 5), (y * 5 + 5), 5, 5);
            }
        }

        g.DrawImage(bitmap, 0, 0, width, height);

        bitmap.Save(fileName);
    }

    private static readonly Dictionary<char?, string> DefaultMap = new()
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