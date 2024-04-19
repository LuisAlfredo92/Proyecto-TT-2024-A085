using System.Globalization;
using General_Data;
using Identifying_data.Names;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Health_data.Clinic_studies;

//https://www.gob.mx/cms/uploads/attachment/file/609372/GUIA_PARA_ESTRUCTURACION_REPORTE_DE_CASO__REVISION_DICTAMEN_2021.pdf
public class ClinicStudiesGenerator
{
    private const string PathToExample = "./Clinic studies/{0}.pdf";
    private static PdfWriter? _pdfWriter;
    private static readonly PdfFont TitlesFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
    private static readonly PdfFont TextFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
    private static readonly Style TitlesStyle = new Style().SetFont(TitlesFont).SetFontSize(14).SetBold();
    private static readonly Style BoldText = new Style().SetFont(TitlesFont).SetFontSize(12).SetBold();
    private static readonly Style RegularText = new Style().SetFont(TextFont).SetFontSize(12);

    public static string Generate()
    {
        string fileName = StringGenerator.GenerateString(12),
            filePath = string.Format(PathToExample, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var fileStream = File.Create(filePath);
        _pdfWriter = new PdfWriter(fileStream);
        PdfDocument pdfDocument = new(_pdfWriter);
        using Document document = new(pdfDocument);

        #region General data
        // Expedient information
        Table expedientTable = new(3, true);
        expedientTable.AddCell(new Cell().Add(new Paragraph("Versión: " + Random.Shared.Next()).AddStyle(TitlesStyle)));
        expedientTable.AddCell(new Cell().Add(new Paragraph("Fecha: " + DateTime.Now.ToString(CultureInfo.CurrentCulture)).AddStyle(TitlesStyle)));
        expedientTable.AddCell(new Cell().Add(new Paragraph("Sede: " + Random.Shared.Next()).AddStyle(TitlesStyle)));
        document.Add(expedientTable);
        document.Add(new Paragraph());

        // Participants
        document.Add(WriteValuesParagraph("Autor: ", NamesGenerator.Generate()));
        document.Add(WriteValuesParagraph("Revisor: ", NamesGenerator.Generate()));
        #endregion

        #region Case structure
        // Title
        document.Add(new Paragraph("Estudios clínicos").AddStyle(TitlesStyle));

        // Abstract
        document.Add(new Paragraph("Resumen").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Introduction
        document.Add(new Paragraph("Introducción").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Clinical findings
        document.Add(new Paragraph("Hallazgos clínicos").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Therapeutic focus
        document.Add(new Paragraph("Intervención terapéutica").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Follow-up and outcomes
        document.Add(new Paragraph("Seguimiento y resultados").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Discussion
        document.Add(new Paragraph("Discusión").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Recommendations
        document.Add(new Paragraph("Recomendaciones").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));

        // Patient perspective
        document.Add(new Paragraph("Perspectiva del paciente").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(64, 512))).AddStyle(RegularText));
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