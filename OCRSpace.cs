using System.Text;
using System.Text.Json;

namespace Aoc2022;

public class OCRSpace
{
    public static string Decode(string filename)
    {
        var imageData = File.ReadAllBytes(filename);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(Environment.GetEnvironmentVariable("OCRSpaceApiKey")), "apikey");
        content.Add(new StringContent("2"), "OCREngine");
        content.Add(new StringContent("PNG"), "filetype");
        content.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", filename);
        var result = new HttpClient()
            .PostAsync(new Uri("https://api.ocr.space/parse/image"), content)
            .GetAwaiter().GetResult();
        var json = Encoding.UTF8.GetString((result.Content.ReadAsStream()as MemoryStream).ToArray());
        return JsonDocument.Parse(json).RootElement.GetProperty("ParsedResults")[0].GetProperty("ParsedText").ToString();
    }
}