using General_Data;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;
using Identifying_data.Names;
using iText.IO.Image;
using iText.Layout.Element;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace Academic_data.Degrees;

public class DegreeGenerator
{
    private const string PathToExample = "./Degrees/Degree example.pdf";
    private static readonly PdfReader PdfReader = new(PathToExample);
    private static readonly PdfFont Font = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
    private static readonly Style BigText = new Style().SetFont(Font).SetFontSize(20);
    private static readonly Style SmallText = new Style().SetFont(Font).SetFontSize(12);
    private static readonly Style VerySmallText = new Style().SetFont(Font).SetFontSize(6);

    private static readonly string[] Months =
    [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre",
        "Diciembre"
    ];

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Degrees/{fileName}.pdf";
        var fileStream = File.Create(filePath);
        PdfDocument pdfDocument = new(PdfReader, new PdfWriter(fileStream));
        Document document = new(pdfDocument);

        #region Picture

        // Generate random image
        const int width = 130, height = 175;
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
            image.SetFixedPosition(60, 350);
            document.Add(image);
        }

        // Dispose resources
        rawImage.Dispose();

        #endregion

        // Add name
        document.Add(new Paragraph(NamesGenerator.Generate()).AddStyle(BigText).SetFixedPosition(200, 430, 500));

        // Add degree
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(32)).AddStyle(BigText).SetFixedPosition(200, 370, 500));

        // Date data
        string day = Random.Shared.Next(1, 31).ToString("D2"),
            month = Months[Random.Shared.Next(0, 12)],
            year = Random.Shared.Next(10000).ToString("D4");

        #region Date 1

        // Day
        document.Add(new Paragraph(day).AddStyle(BigText).SetFixedPosition(425, 189, 100));
        // Month
        document.Add(new Paragraph(month).AddStyle(BigText).SetFixedPosition(155, 168, 100));
        // Year
        document.Add(new Paragraph(year).AddStyle(BigText).SetFixedPosition(277, 168, 100));

        #endregion

        #region Date 2

        // Day
        document.Add(new Paragraph(day).AddStyle(SmallText).SetFixedPosition(2, 255, 682, 100));
        // Month
        document.Add(new Paragraph(month).AddStyle(SmallText).SetFixedPosition(2, 283, 682, 100));
        // Year
        document.Add(new Paragraph(year).AddStyle(SmallText).SetFixedPosition(2, 352, 683.2f, 100));

        #endregion

        // Fojas
        var fojas = Random.Shared.Next(1, 100).ToString("D2");
        document.Add(new Paragraph(fojas).AddStyle(SmallText).SetFixedPosition(2, 308, 668, 100));

        #region Stamp

        // Fojas
        document.Add(new Paragraph(fojas).AddStyle(VerySmallText).SetFixedPosition(2, 220, 456, 50));
        // Book
        document.Add(new Paragraph(Random.Shared.Next(1, 1000).ToString("D3")).AddStyle(VerySmallText).SetFixedPosition(2, 265, 454, 50));
        // Number
        document.Add(new Paragraph(Random.Shared.Next(1, 1000).ToString("D3")).AddStyle(VerySmallText).SetFixedPosition(2, 219, 444, 50));
        // ID
        document.Add(new Paragraph(Random.Shared.Next(1, 100000000).ToString("D8")).AddStyle(VerySmallText).SetFixedPosition(2, 218, 436, 50));

        #region Date 3

        // Day
        document.Add(new Paragraph(day).AddStyle(VerySmallText).SetFixedPosition(201, 430, 50).SetPageNumber(2));
        // Month
        document.Add(new Paragraph(month).AddStyle(VerySmallText).SetFixedPosition(214, 429, 50).SetPageNumber(2));
        // Year
        document.Add(new Paragraph(year).AddStyle(VerySmallText).SetFixedPosition(241, 428, 50).SetPageNumber(2));

        #endregion

        #endregion

        #region Sign

        // Generate random image
        const int widthSign = 100, heightSign = 50;
        pixels = new Rgb24[widthSign * heightSign];
        bytes = stackalloc byte[widthSign * 3];
        for (var i = 0; i < heightSign; i++)
        {
            RandomNumberGenerator.Fill(bytes);
            var pixelsRow = pixels.Slice(i * widthSign, widthSign);
            PixelOperations<Rgb24>.Instance.FromRgb24Bytes(Configuration.Default, bytes, pixelsRow, widthSign);
        }

        // Convert image to grayscale
        rawImage = Image.LoadPixelData<Rgb24>(pixels, widthSign, heightSign);
        using (MemoryStream saveStream = new())
        {
            rawImage.Mutate(x => x.Grayscale());
            rawImage.SaveAsJpeg(saveStream);
            saveStream.Position = 0;

            // Add image to document
            var image = new iText.Layout.Element.Image(ImageDataFactory.CreateJpeg(saveStream.ToArray()))
                .SetFixedPosition(140, 600)
                .SetPageNumber(2);
            document.Add(image);
        }

        // Dispose resources
        rawImage.Dispose();

        #endregion

        document.Close();
        return filePath;
    }
}