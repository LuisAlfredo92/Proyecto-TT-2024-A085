using System.Globalization;
using General_Data;
using Identifying_data.Exterior_numbers;
using Identifying_data.Localities;
using Identifying_data.Names;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Health_data.Clinic_historical;

public class ClinicHistoricalGenerator
{
    private const string PathToExample = "./Clinic historical/{0}.pdf";
    private static PdfWriter? _pdfWriter;
    private static readonly PdfFont TitlesFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
    private static readonly PdfFont TextFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
    private static readonly Style TitlesStyle = new Style().SetFont(TitlesFont).SetFontSize(14).SetBold();
    private static readonly Style BoldText = new Style().SetFont(TitlesFont).SetFontSize(12).SetBold();
    private static readonly Style RegularText = new Style().SetFont(TextFont).SetFontSize(12);
    private static readonly string[] CivilStatus = ["Soltero", "Casado", "Divorciado", "Viudo"];
    private const string AddressTemplate = "{0} {1}, {2}, {3}, {4}";

    public static string GenerateStudies()
    {
        string fileName = StringGenerator.GenerateString(12),
            filePath = string.Format(PathToExample, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var fileStream = File.Create(filePath);
        _pdfWriter = new PdfWriter(fileStream);
        PdfDocument pdfDocument = new(_pdfWriter);
        using Document document = new(pdfDocument);

        // Title
        document.Add(new Paragraph("Estudios clínicos").AddStyle(TitlesStyle));
        document.Add(new Paragraph());

        #region Personal data
        document.Add(new Paragraph("Datos personales").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Nombre y Apellido: ", NamesGenerator.Generate()));
        document.Add(WriteValuesParagraph("Edad: ", Random.Shared.Next(1, 100) + " años"));
        document.Add(WriteValuesParagraph("Estado civil: ", CivilStatus[Random.Shared.Next(0, CivilStatus.Length)]));
        document.Add(WriteValuesParagraph("Nacionalidad: ", StringGenerator.GenerateString(12)));
        document.Add(WriteValuesParagraph("Ocupación: ", StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(12, 32))));
        var address = string.Format(AddressTemplate,
            StreetNamesGenerator.Generate(),
            ExteriorNumbersGenerator.GenerateHouseNumber(),
            SettlementsGenerator.Generate(),
            LocalitiesGenerator.Generate(),
            StatesGenerator.Generate());
        document.Add(WriteValuesParagraph("Domicilio: ", address));
        document.Add(WriteValuesParagraph("Persona responsable: ", NamesGenerator.Generate()));
        #endregion
        document.Add(new Paragraph());

        #region Consultation reason
        document.Add(new Paragraph("Motivo de consulta").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        #endregion
        document.Add(new Paragraph());

        #region Actual disease and previous
        document.Add(new Paragraph("Enfermedad actual y sus antecedentes").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 512))).AddStyle(RegularText));
        #endregion
        document.Add(new Paragraph());

        #region Personal antecedents
        document.Add(new Paragraph("Antecedentes personales").AddStyle(TitlesStyle));
        // Fisiological
        document.Add(new Paragraph("Fisiológicos:").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Menarga: ", Random.Shared.Next(1, 100) + " años"));
        int min = Random.Shared.Next(1,31), max = Random.Shared.Next(min, 32);
        document.Add(WriteValuesParagraph("Ritmo actual: ", $"{min}-{max}/{Random.Shared.Next(1, 10)} días"));
        document.Add(WriteValuesParagraph("Fecha de la última menstruación: ", DatesGenerator.GenerateDate(DateTime.MinValue, DateTime.MaxValue).ToString(CultureInfo.CurrentCulture)));
        document.Add(WriteValuesParagraph("Inicio de las relaciones sexuales: ", $"{Random.Shared.Next(15, 50)} años"));
        document.Add(WriteValuesParagraph("Gestaciones, partos y abortos: ", $"G{Random.Shared.Next(0, 5)} P{Random.Shared.Next(0, 5)} A{Random.Shared.Next(0, 5)}"));
        document.Add(WriteValuesParagraph("Inmunizaciones: ", StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(4, 32))));
        
        // Pathological
        document.Add(new Paragraph());
        document.Add(new Paragraph("Patológicos:").AddStyle(TitlesStyle));
        document.Add(new Paragraph("Enfermedades de la infancia:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Enfermedades clínicas:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Antecedentes alérgicos:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Antecedentes quirúrgicos y traumáticos:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        #endregion

        document.Add(new Paragraph());

        #region Medium antecedents
        document.Add(new Paragraph("Antecedentes de medio:").AddStyle(TitlesStyle));
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Hábitos:").AddStyle(TitlesStyle));
        document.Add(WriteValuesParagraph("Alimentación: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Apetito: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Catarsis intestinal: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Diuresis: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Sueño: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Bebidas alcoholicas: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Infusiones: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Drogas: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Tabaco: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Medicamentos: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Hábitos sexuales: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Actividad física: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        #endregion
        document.Add(new Paragraph());

        #region Family antecedents
        document.Add(new Paragraph("Antecedentes familiares").AddStyle(TitlesStyle));
        document.Add(WriteValuesParagraph("Padre: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        document.Add(WriteValuesParagraph("Madre: ", StringGenerator.GenerateString(Random.Shared.Next(8, 64))));
        #endregion

        // Anamnesis
        #region Phisical exam
        document.Add(new Paragraph("Impresión general:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Piel y faneras:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Sistema celular subcutáneo:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Sistema linfático:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Sistema venoso superficial:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Sistema osteoarticulomuscular:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Cabeza:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Cuello:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Tórax").AddStyle(TitlesStyle));
        document.Add(new Paragraph("Aparato respiratorio:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Aparato circulatorio:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Abdomen:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Aparato genital:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
        document.Add(new Paragraph());
        document.Add(new Paragraph("Sistema nervioso:").AddStyle(TitlesStyle)).SetItalic();
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 256))).AddStyle(RegularText));
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