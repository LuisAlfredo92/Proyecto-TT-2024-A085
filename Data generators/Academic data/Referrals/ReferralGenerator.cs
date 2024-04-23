using General_Data;
using Identifying_data.Curps;
using Identifying_data.Names;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Security.Cryptography;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace Academic_data.Referrals;

public class ReferralGenerator
{
    private const string PathToExample = "./Referrals/Referral example.pdf";
    private static readonly PdfReader PdfReader = new(PathToExample);
    private static readonly PdfFont Font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

    private static readonly Style SmallText =
        new Style().SetFont(Font).SetFontSize(10);

    private static readonly Style VerySmallText = new Style().SetFont(Font).SetFontSize(6);

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Referrals/{fileName}.pdf";
        var fileStream = File.Create(filePath);
        PdfDocument pdfDocument = new(PdfReader, new PdfWriter(fileStream));
        Document document = new(pdfDocument);

        // Referral number
        document.Add(new Paragraph(Random.Shared.NextInt64(9999999999).ToString("D10"))
            .AddStyle(SmallText)
            .SetFixedPosition(390, 605, 250));

        // CURP
        document.Add(new Paragraph(CurpsGenerator.Generate())
            .AddStyle(SmallText)
            .SetFixedPosition(345, 565, 250));

        // Book
        document.Add(new Paragraph(Random.Shared.NextInt64(999).ToString("D3"))
            .AddStyle(SmallText)
            .SetFixedPosition(333, 504, 250));

        // Foja
        document.Add(new Paragraph(Random.Shared.NextInt64(999).ToString("D3"))
            .AddStyle(SmallText)
            .SetFixedPosition(380, 504, 250));

        // Number
        document.Add(new Paragraph(Random.Shared.NextInt64(100).ToString("D2"))
                   .AddStyle(SmallText)
                   .SetFixedPosition(430, 504, 250));

        // Name
        document.Add(new Paragraph(NamesGenerator.GetName())
                   .AddStyle(SmallText)
                   .SetFixedPosition(90, 458, 250));

        // First last name
        document.Add(new Paragraph(NamesGenerator.GetLastName())
                          .AddStyle(SmallText)
                          .SetFixedPosition(220, 458, 250));

        // Second last name
        document.Add(new Paragraph(NamesGenerator.GetLastName())
                                 .AddStyle(SmallText)
                                 .SetFixedPosition(350, 458, 250));

        // Degree
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(32))
                   .AddStyle(SmallText)
                   .SetFixedPosition(65, 365, 250));

        // Degree key
        document.Add(new Paragraph(Random.Shared.NextInt64(1000000).ToString("D6"))
                          .AddStyle(SmallText)
                          .SetFixedPosition(361, 365, 250));

        // School name
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(32))
            .AddStyle(SmallText)
            .SetFixedPosition(65, 298, 250));

        // School key
        document.Add(new Paragraph(Random.Shared.NextInt64(1000000).ToString("D6"))
            .AddStyle(SmallText)
            .SetFixedPosition(361, 298, 250));

        // Date
        var date = DateTime.FromBinary(Random.Shared.NextInt64(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks));
        document.Add(new Paragraph(date.ToString("dd/MM/yyyy"))
                   .AddStyle(SmallText)
                   .SetFixedPosition(105, 247, 250));
        // Time
        document.Add(new Paragraph(date.ToString("HH:mm:ss"))
                          .AddStyle(SmallText)
                          .SetFixedPosition(362, 247, 250));

        // Original string
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(256))
                   .AddStyle(VerySmallText)
                   .SetFixedPosition(30, 130, 300));

        // Electronic signature
        document.Add(new Paragraph(StringGenerator.GenerateString(256))
                          .AddStyle(VerySmallText)
                          .SetFixedPosition(30, 87, 250));

        // Digital signature
        document.Add(new Paragraph(StringGenerator.GenerateString(64))
                                 .AddStyle(VerySmallText)
                                 .SetFixedPosition(30, 70, 250));

        #region QR code

        // Generate random image
        const int width = 50, height = 50;
        Span<Rgb24> pixels = new Rgb24[width * height];
        Span<byte> bytes = stackalloc byte[width * 3];
        for (var i = 0; i < height; i++)
        {
            RandomNumberGenerator.Fill(bytes);
            var pixelsRow = pixels.Slice(i * width, width);
            PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, pixelsRow, width);
        }

        // Convert image to grayscale
        var rawImage = Image.LoadPixelData<Rgb24>(pixels, width, height);
        using (MemoryStream saveStream = new())
        {
            rawImage.Mutate(x => x.Grayscale());
            rawImage.SaveAsJpeg(saveStream);
            saveStream.Position = 0;

            // Add image to document
            var image = new iText.Layout.Element.Image(ImageDataFactory.CreateJpeg(saveStream.ToArray()));
            image.SetFixedPosition(398, 20);
            document.Add(image);
        }

        // Dispose resources
        rawImage.Dispose();

        #endregion

        document.Close();
        return filePath;
    }
}