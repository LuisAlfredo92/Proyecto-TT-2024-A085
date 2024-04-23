using General_Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Processing;

namespace Biometric_data.Iris;

public class IrisGenerator
{
    private const int Width = 640,
        Height = 480,
        TotalPixels = Width * Height,
        CenterX = 320,
        CenterY = 240;

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Iris/{fileName}.bmp";
        Directory.CreateDirectory("./Iris");
        var fileStream = File.Create(filePath);

        var rawImage = GenerateRawImage();
        rawImage.Mutate(x => x.Rotate(RotateMode.Rotate180));
        var polarImage = ConvertToPolarCoordinates(rawImage);

        polarImage.Save(fileStream, new BmpEncoder());

        return filePath;
    }

    private static Image<Rgb24> GenerateRawImage()
    {
        // 640 * 480 pixels
        Span<Rgb24> pixels = new Rgb24[TotalPixels];
        Span<byte> bytes = stackalloc byte[Width * 3];

        // Generate top border 0 - 99
        for (var i = 0; i < 100; i++)
        {
            bytes.Fill((byte)(255 - 2.575757 * i));
            var pixelsRow = pixels.Slice(i * Width, Width);
            PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, pixelsRow, Width);
        }

        // Generate middle 100 - 400
        var frequency = Random.Shared.NextSingle();
        for (var i = 100; i < 400; i++)
        {
            var pixelsRow = pixels.Slice(i * Width, Width);
            for (var j = 0; j < Width; j++)
            {
                var colorBytes = bytes.Slice(j * 3, 3);
                double amplitude = Height * 0.5f * Math.Sin(2 * Math.PI * frequency * j / Width) + Height * 0.5f,
                    value = amplitude * Math.Sin(2 * Math.PI * frequency * j);
                colorBytes.Fill((byte)(value + 128));
            }

            PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, pixelsRow, Width);
        }

        // Generate pupil 400 - 479
        // First we do a tiny gray line to avoid the pupil to be a perfect circle
        for (var i = 400; i < 409; i++)
        {
            bytes.Fill((byte)(150 - 5 * (i - 400)));
            var pixelsRow = pixels.Slice(i * Width, Width);
            PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, pixelsRow, Width);
        }

        var finalPixelsRow = pixels[(410 * Width)..];
        bytes = new byte[finalPixelsRow.Length * 3];
        PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, finalPixelsRow,
            finalPixelsRow.Length);

        // Create image
        return Image.LoadPixelData<Rgb24>(new Configuration(new BmpConfigurationModule()), pixels, Width, Height);
    }

    // https://github.com/SixLabors/ImageSharp/discussions/2724
    private static Image<Rgb24> ConvertToPolarCoordinates(Image<Rgb24> image)
    {
        Image<Rgb24> polarImage = new(Width, Height);
        var center = new PointF(CenterX, CenterY);
        const float maxRadius = CenterY;

        polarImage.Mutate(ctx =>
        {
            ctx.ProcessPixelRowsAsVector4((pixelRow, offset) =>
            {
                for (var x = 0; x < pixelRow.Length; x++)
                {
                    // Map to polar coordinates
                    float dx = x - center.X,
                        dy = offset.Y - center.Y,
                        radius = MathF.Sqrt(dx * dx + dy * dy) / maxRadius,
                        theta = MathF.Atan2(dy, dx) - MathF.PI * 0.5f; // Subtract π/2 for 90-degree clockwise rotation

                    // Normalize theta to range from 0 to 1
                    theta = (theta + MathF.PI) / (2 * MathF.PI);
                    while (theta < 0)
                        theta += 1; // Wrap around if negative

                    if (radius > 1f) continue; // Only process pixels within the circle

                    // Map radius and theta back to source coordinates
                    int sourceX = (int)(theta * Width),
                        sourceY = (int)(radius * Height);
                    sourceX = Math.Clamp(sourceX, 0, Width - 1);
                    sourceY = Math.Clamp(sourceY, 0, Height - 1);

                    pixelRow[x] = image[sourceX, sourceY].ToScaledVector4();
                }
            }, PixelConversionModifiers.Scale);
        });

        return polarImage;
    }

}