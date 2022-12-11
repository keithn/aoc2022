namespace Aoc2022;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class TextToImage
{
    public static void From(List<string> lines, string fileName)
    {
        var width = lines.Max(l => l.Length) * 10;
        var height = lines.Count * 5+10;
        var bitmap = new Bitmap(width, height);
        using var g = Graphics.FromImage(bitmap);
        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (c == '#')
                {
                    g.FillRectangle(Brushes.White, 5+(x*5), (y*5+5), 5, 5);
                }
            }
        }
        g.DrawImage(bitmap, 0,0, width,height);
        
        bitmap.Save(fileName);
    }
}