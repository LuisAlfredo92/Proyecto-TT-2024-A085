using Academic_data.Cct;
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

namespace Academic_data.Report_cards;

public class ReportCardsGenerator
{
    private const string PathToExample = "./Report cards/Report card example.pdf";
    private static readonly PdfReader PdfReader = new(PathToExample);
    private static readonly PdfFont Font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
    private static readonly Style Text = new Style().SetFont(Font).SetFontSize(8);

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Report cards/{fileName}.pdf";
        using var fileStream = File.Create(filePath);
        PdfDocument pdfDocument = new(PdfReader, new PdfWriter(fileStream));
        Document document = new(pdfDocument);
        string name = NamesGenerator.Generate(),
            curp = CurpsGenerator.Generate(),
            schoolName = StringGenerator.GenerateStringWithSpaces(50),
            cct = CctGenerator.GenerateCct();
        Span<float> finalGrades = stackalloc float[6];
        const int width = 55, height = 38, pixelsLength = width * height;
        Span<Rgb24> pixels = new Rgb24[pixelsLength];
        Span<byte> bytes = stackalloc byte[pixelsLength];

        for (var pageNumber = 1; pageNumber <= 6; pageNumber++)
        {
            // Name
            document.Add(new Paragraph(name).AddStyle(Text).SetFixedPosition(pageNumber, 203, 674, 250));

            // CURP
            document.Add(new Paragraph(curp).AddStyle(Text).SetFixedPosition(pageNumber, 470, 674, 275));

            // School name
            document.Add(new Paragraph(schoolName).AddStyle(Text).SetFixedPosition(pageNumber, 102, 651, 300));

            // CCT
            document.Add(new Paragraph(cct).AddStyle(Text).SetFixedPosition(pageNumber, 381, 650, 150));

            // Schedule
            var schedule = Random.Shared.NextSingle() < 0.5 ? "Matutino" : "Vespertino";
            document.Add(new Paragraph(schedule).AddStyle(Text).SetFixedPosition(pageNumber, 484, 650, 150));

            // Group
            var group = Random.Shared.Next(3) switch
            {
                0 => "A",
                1 => "B",
                _ => "C"
            };
            document.Add(new Paragraph(group).AddStyle(Text).SetFixedPosition(pageNumber, 562, 650, 150));

            // Indigenous dialect
            document.Add(new Paragraph("N/A").AddStyle(Text).SetFixedPosition(pageNumber, 437, 624, 150));

            // Promoted
            if(pageNumber > 1)
                document.Add(new Paragraph("X").AddStyle(Text).SetBold().SetFixedPosition(pageNumber, 435, 588, 150));

            // Assistance
            var bottom = pageNumber == 1 ? 570 : 552;
            document.Add(new Paragraph(Random.Shared.Next(160, 200).ToString()).AddStyle(Text).SetFixedPosition(pageNumber, 480, bottom, 150));

            // Grades
            // Create grades
            var grades = new float[4, 4];
            const int gradesInitialX = 80, gradesWidthX = 75;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    grades[i, j] = Math.Clamp(Random.Shared.Next(6, 11) + Random.Shared.NextSingle(), 6, 10);
                    document.Add(new Paragraph(grades[i, j].ToString("0.00")).AddStyle(Text).SetFixedPosition(pageNumber, gradesInitialX + j * gradesWidthX, 590 - i * 25, 150));
                }
            }
            // Calculate averages per column
            for (var j = 0; j < 4; j++)
            {
                grades[3, j] = (grades[0,j] + grades[1,j] + grades[2,j]) * 0.33333333333333f;
                document.Add(new Paragraph(grades[3, j].ToString("0.00")).AddStyle(Text).SetFixedPosition(pageNumber, gradesInitialX + j * gradesWidthX, 517, 150));
            }

            // Final average
            var finalAverage = (grades[3, 0] + grades[3, 1] + grades[3, 2] + grades[3, 3]) * 0.25f;
            finalGrades[pageNumber - 1] = finalAverage;
            if(pageNumber < 6)
                document.Add(new Paragraph(finalAverage.ToString("0.00")).AddStyle(Text).SetFixedPosition(pageNumber, 492, 517, 150));

            // Observations
            const float observationsInitialX = 55f, observationsInitialY = 450f, observationsWidthY = 125f;
            for (var i = 0; i < 3; i++)
            {
                // It's a little rare to have observations, so to a more realistic distribution, we'll use a 25% chance
                //if (Random.Shared.NextSingle() >= 0.25f) continue;
                var observation = StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 512));
                document.Add(new Paragraph(observation).AddStyle(Text).SetFixedPosition(pageNumber, observationsInitialX, observationsInitialY - i * observationsWidthY, 500));
            }

            // Signs
            const float signsInitialX = 385f, signsWidthX = 67f;
            for (var i = 0; i < 3; i++)
            {
                // Generate random image
                for (var j = 0; j < height; j++)
                {
                    RandomNumberGenerator.Fill(bytes);
                    var pixelsRow = pixels.Slice(j * width, width);
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
                    image.SetFixedPosition(pageNumber, signsInitialX + i * signsWidthX, 65);
                    document.Add(image);
                }

                // Dispose resources
                rawImage.Dispose();
            }
        }

        // Final school average
        float total = 0;
        var enumerable = finalGrades.ToArray();
        Parallel.ForEach(enumerable, grade => total += grade);
        var schoolAverage = total * 0.16666666667f;
        document.Add(new Paragraph(finalGrades[5].ToString("0.00")).AddStyle(Text).SetFixedPosition(6, 433, 517, 150));
        document.Add(new Paragraph(schoolAverage.ToString("0.00")).AddStyle(Text).SetFixedPosition(6, 553, 517, 150));

        document.Close();
        return filePath;
    }
}