using Biometric_data.Skin_color;
using General_Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Biometric_data.Scars;

public class ScarsGenerator
{
    private const int Width = 1920,
        Height = 1080,
        CenterX = (int)(Width * 0.5),
        CenterY = (int)(Height * 0.5);

    public static string GenerateLocation() => StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(8, 51));

    public static float GenerateSize() => Random.Shared.NextSingle() * 30;

    public static string GeneratePhoto()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Scars/{fileName}.jpg";
        Directory.CreateDirectory("./Scars");
        using var fileStream = File.Create(filePath);

        string skinColorString = SkinColorGenerator.GetPerlaSkinColor(),
            scarColorString;
        do
        {
            scarColorString = SkinColorGenerator.GetPerlaSkinColor();
        } while (scarColorString == skinColorString);
        Color skinColor = Color.ParseHex(skinColorString),
            scarColor = Color.ParseHex(scarColorString);
        Image<Rgb24> image = new(Width, Height, skinColor);

        // Generate shape
        var vertices = Random.Shared.Next(16, 32);
        var differenceGrades = 360f / vertices;
        var scarShapes = new IPath[vertices + 1];

        for (var i = 0; i < vertices; i++)
        {
            float grades = i * differenceGrades + (Random.Shared.NextSingle() * differenceGrades - differenceGrades * 0.5f),
                magnitude = Math.Max(Random.Shared.NextSingle(), 0.5f) * 500f,
                x = (float)(Math.Cos(grades * 0.01745329) * magnitude),
                y = (float)(Math.Sin(grades * 0.01745329) * magnitude);
            PointF center = new(CenterX + x, CenterY + y);
            float width = MathF.Max(Random.Shared.NextSingle(), 0.5f) * 200f,
                height = MathF.Max(Random.Shared.NextSingle(), 0.5f) * 200f;
            SizeF size = new(width, height);
            scarShapes[i] = new SixLabors.ImageSharp.Drawing.Path(new ArcLineSegment(center, size, Random.Shared.Next(360), 0, 360));
        }

        scarShapes[vertices] = new SixLabors.ImageSharp.Drawing.Path(
            new ArcLineSegment(new PointF(CenterX, CenterY),
            new SizeF(256, 256),
            0,
            0,
            360));

        image.Mutate(x => x.Fill(scarColor, new PathCollection(scarShapes)));
        image.SaveAsJpeg(fileStream);

        return filePath;
    }
}