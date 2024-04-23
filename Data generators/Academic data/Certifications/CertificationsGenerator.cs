using Academic_data.Cct;
using General_Data;
using Identifying_data.Curps;
using Identifying_data.Exterior_numbers;
using Identifying_data.Localities;
using Identifying_data.Municipalities;
using Identifying_data.Names;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Academic_data.Certifications;

public class CertificationsGenerator
{
    private const string PathToExample = "./Certifications/Certificado secundaria.pdf";
    private static readonly PdfReader PdfReader = new(PathToExample);
    private static readonly PdfFont Font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
    private static readonly Style Text = new Style().SetFont(Font).SetFontSize(8);

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Certifications/{fileName}.pdf";
        using var fileStream = File.Create(filePath);
        PdfDocument pdfDocument = new(PdfReader, new PdfWriter(fileStream));
        Document document = new(pdfDocument);

        // Name
        document.Add(new Paragraph(NamesGenerator.Generate()).AddStyle(Text).SetFixedPosition(225, 510, 300));

        // CURP
        document.Add(new Paragraph(CurpsGenerator.Generate()).AddStyle(Text).SetFixedPosition(262, 471, 300));

        // School name
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(64)).AddStyle(Text).SetFixedPosition(185, 418, 300));

        // CCT
        document.Add(new Paragraph(CctGenerator.GenerateCct()).AddStyle(Text).SetFixedPosition(145, 381, 300));

        // Grade
        document.Add(new Paragraph(Math.Clamp(Random.Shared.Next(6, 10) + Random.Shared.NextSingle(), 6, 10).ToString("0.00")).AddStyle(Text).SetFixedPosition(430, 381, 50));

        // Place
        var address = $"{StreetNamesGenerator.Generate()}, " +
                      $"{ExteriorNumbersGenerator.GenerateHouseNumber()}, " +
                      $"{SettlementsGenerator.Generate()}, " +
                      $"{LocalitiesGenerator.Generate()}, " +
                      $"{MunicipalitiesGenerator.Generate()}, " +
                      $"{StatesGenerator.Generate()}";
        document.Add(new Paragraph(address).AddStyle(Text).SetFixedPosition(66, 328, 500));

        // Date
        document.Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy")).AddStyle(Text).SetFixedPosition(291, 291, 150));

        // Number
        document.Add(new Paragraph(Random.Shared.Next(1, 1000000).ToString("D6")).AddStyle(Text).SetFixedPosition(72, 30, 50));

        document.Close();
        return filePath;
    }
}