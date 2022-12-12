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
        { 'a', "367F09" },
        { 'b', "3B8110" },
        { 'c', "408316" },
        { 'd', "45851D" },
        { 'e', "4A8724" },
        { 'f', "4E892A" }, { 'g', "538B31" }, { 'h', "588D38" }, { 'i', "5D8F3E" }, { 'j', "629145" }, { 'k', "67934C" },
        { 'l', "6C9552" }, { 'm', "719759" }, { 'n', "759860" }, { 'o', "7A9A67" }, { 'p', "7F9C6D" }, { 'q', "849E74" }, { 'r', "89A07B" },
        { 's', "8EA281" }, { 't', "93A488" }, { 'u', "98A68F" }, { 'v', "9CA895" }, { 'w', "A1AA9C" }, { 'x', "A6ACA3" }, { 'y', "ABAEA9" },
        { 'z', "B0B0B0" },
        { 'S', "FF0000" },
        { 'P', "AAAA0000" },
        { 'E', "AAAA0000" },
    };
}