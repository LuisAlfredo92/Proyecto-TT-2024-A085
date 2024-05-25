using System.Globalization;
using General_Data;
using Identifying_data.Born_dates;
using Identifying_data.Names;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Health_data.Clinic_studies;

// https://es.scribd.com/document/151841469/Analisis-Clinicos-Reportes
public class ClinicStudiesGenerator
{
    private const string PathToFile = "./Clinic studies/{0}.pdf";
    private static PdfWriter? _pdfWriter;
    private static readonly PdfFont TitlesFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
    private static readonly PdfFont TextFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
    private static readonly Style HeaderStyle = new Style().SetFont(TitlesFont).SetFontSize(24).SetBold().SetTextAlignment(TextAlignment.CENTER);
    private static readonly Style TitlesStyle = new Style().SetFont(TitlesFont).SetFontSize(14).SetBold().SetTextAlignment(TextAlignment.CENTER);
    private static readonly Style BoldText = new Style().SetFont(TitlesFont).SetFontSize(12).SetBold();
    private static readonly Style RegularText = new Style().SetFont(TextFont).SetFontSize(12);

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(12),
            filePath = string.Format(PathToFile, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var fileStream = File.Create(filePath);
        _pdfWriter = new PdfWriter(fileStream);
        PdfDocument pdfDocument = new(_pdfWriter);
        using Document document = new(pdfDocument);

        #region Header

        Table headerTable = new(3, true);
        var imageData = ImageDataFactory.Create("./Clinic studies/Microscope.png");
        Image leftImage = new(imageData),
            rightImage = new(imageData);
        leftImage.SetWidth(50);
        rightImage.SetWidth(50).SetHorizontalAlignment(HorizontalAlignment.RIGHT);
        headerTable.AddCell(new Cell().Add(leftImage).SetBorder(Border.NO_BORDER));
        headerTable.AddCell(new Cell().Add(new Paragraph("Estudios clínicos").AddStyle(HeaderStyle)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));
        headerTable.AddCell(new Cell().Add(rightImage).SetBorder(Border.NO_BORDER));
        document.Add(headerTable);

        #endregion

        #region Document data

        document.Add(new Paragraph("Datos del paciente").AddStyle(TitlesStyle));
        Table dataTable = new(2, true);

        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Nombre: ", NamesGenerator.Generate())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Fecha de muestra: ", DatesGenerator.GenerateDate(DateTime.MinValue, DateTime.MaxValue).ToString(CultureInfo.InvariantCulture))).SetBorder(Border.NO_BORDER));
        var bornDate = BornDatesGenerator.GenerateBornDate();
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Fecha de nacimiento: ", bornDate.ToString(CultureInfo.InvariantCulture))).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Registro: ", Random.Shared.Next().ToString())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Edad: ", (DateTime.Now.Year - bornDate.Year + (DateTime.Now.DayOfYear < bornDate.DayOfYear ? -1 :  0)).ToString())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Sexo: ", Random.Shared.Next(2) == 0 ? "Masculino" : "Femenino")).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Peso: ", Random.Shared.Next(50, 180).ToString())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Altura: ", Random.Shared.Next(150, 200).ToString())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().Add(WriteValuesParagraph("Médico: ", NamesGenerator.Generate())).SetBorder(Border.NO_BORDER));
        dataTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));

        document.Add(dataTable);

        #endregion

        #region Studies

        document.Add(new Paragraph("Resultados").AddStyle(TitlesStyle));
        Table studiesTable = new(4, true);

        // Headers
        Color headerColor = new DeviceRgb(0, 255, 255);
        studiesTable.AddCell(new Cell().Add(new Paragraph("Parámetro").AddStyle(BoldText)).SetBackgroundColor(headerColor));
        studiesTable.AddCell(new Cell().Add(new Paragraph("Resultado").AddStyle(BoldText)).SetBackgroundColor(headerColor));
        studiesTable.AddCell(new Cell().Add(new Paragraph("Unidad").AddStyle(BoldText)).SetBackgroundColor(headerColor));
        studiesTable.AddCell(new Cell().Add(new Paragraph("Valores de referencia").AddStyle(BoldText)).SetBackgroundColor(headerColor));

        // Results
        var limit = Random.Shared.Next(1, 11);
        for (var i = 0; i < limit; i++)
        {
            studiesTable.AddCell(new Cell().Add(new Paragraph(StringGenerator.GenerateString(10))));
            studiesTable.AddCell(new Cell().Add(new Paragraph(Random.Shared.Next(100).ToString())));
            studiesTable.AddCell(new Cell().Add(new Paragraph(StringGenerator.GenerateString(3))));
            studiesTable.AddCell(new Cell().Add(new Paragraph($"{Random.Shared.Next(100)}-{Random.Shared.Next(100)}")));
        }

        document.Add(studiesTable);

        document.Add(new Paragraph("Observaciones:").AddStyle(BoldText));
        document.Add(new Paragraph(StringGenerator.GenerateString(Random.Shared.Next(0, 256))).AddStyle(RegularText));

        #endregion

        #region Footer

        const string footerMsg =
            "IMPORTANTE: Los resultados incluidos en este reporte no sustituyen la consulta médica. Para una interpretación adecuada,es necesario que un médico los revise y coleccione con información clínica (signos, síntomas, antecedentes) y la obtenida deotras pruebas complementarias";
        document.Add(new Paragraph(footerMsg).AddStyle(BoldText).SetTextAlignment(TextAlignment.JUSTIFIED));

        #endregion

        return filePath;
    }

    private static Paragraph WriteValuesParagraph(string tag, string text)
    {
        var paragraph = new Paragraph(tag).AddStyle(BoldText);
        paragraph.Add(new Text(text).AddStyle(RegularText));
        return paragraph;
    }
}