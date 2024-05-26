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
    private const string PathToFile = "./Clinic historical/{0}.pdf";
    private static PdfWriter? _pdfWriter;
    private static readonly PdfFont TitlesFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
    private static readonly PdfFont TextFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
    private static readonly Style HeaderStyle = new Style().SetFont(TitlesFont).SetFontSize(24).SetBold();
    private static readonly Style TitlesStyle = new Style().SetFont(TitlesFont).SetFontSize(18).SetBold();
    private static readonly Style BoldText = new Style().SetFont(TitlesFont).SetFontSize(12).SetBold();
    private static readonly Style RegularText = new Style().SetFont(TextFont).SetFontSize(12);
    private static readonly string[] CivilStatus = ["Soltero", "Casado", "Divorciado", "Viudo"];
    private const string AddressTemplate = "{0} {1}, {2}, {3}, {4}";

    public static string GenerateStudies()
    {
        string fileName = StringGenerator.GenerateString(12),
            filePath = string.Format(PathToFile, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var fileStream = File.Create(filePath);
        _pdfWriter = new PdfWriter(fileStream);
        PdfDocument pdfDocument = new(_pdfWriter);
        using Document document = new(pdfDocument);

        // Title
        document.Add(new Paragraph("Historial clínico").AddStyle(HeaderStyle));
        document.Add(new Paragraph(DateTime.Now.ToString(CultureInfo.CurrentCulture)).AddStyle(RegularText));
        document.Add(new Paragraph());

        #region Clinic data
        document.Add(new Paragraph("Datos del establecimiento").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Tipo: ", StringGenerator.GenerateString(12)));
        document.Add(WriteValuesParagraph("Nombre: ", StringGenerator.GenerateStringWithSpaces(16)));
        var clinicAddress = string.Format(AddressTemplate,
            StreetNamesGenerator.Generate(),
            ExteriorNumbersGenerator.GenerateHouseNumber(),
            SettlementsGenerator.Generate(),
            LocalitiesGenerator.Generate(),
            StatesGenerator.Generate());
        document.Add(WriteValuesParagraph("Domicilio: ", clinicAddress));
        #endregion
        document.Add(new Paragraph());

        #region Personal data
        document.Add(new Paragraph("Datos personales").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Nombre: ", NamesGenerator.Generate()));
        document.Add(WriteValuesParagraph("Sexo: ", Random.Shared.Next(0, 2) == 0 ? "Masculino" : "Femenino"));
        document.Add(WriteValuesParagraph("Edad: ", Random.Shared.Next(1, 100) + " años"));
        document.Add(WriteValuesParagraph("Estado civil: ", CivilStatus[Random.Shared.Next(0, CivilStatus.Length)]));
        var address = string.Format(AddressTemplate,
            StreetNamesGenerator.Generate(),
            ExteriorNumbersGenerator.GenerateHouseNumber(),
            SettlementsGenerator.Generate(),
            LocalitiesGenerator.Generate(),
            StatesGenerator.Generate());
        #endregion
        document.Add(new Paragraph());

        #region Medic data
        document.Add(new Paragraph("Datos del médico").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Nombre: ", NamesGenerator.Generate()));
        document.Add(WriteValuesParagraph("Edad: ", Random.Shared.Next(1, 100) + " años"));
        #endregion
        document.Add(new Paragraph());

        #region Clinic history
        document.Add(new Paragraph("Historia clínica").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Interrogatorio", string.Empty));
        document.Add(WriteValuesParagraph("Ficha de identificación: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Grupo étnico: ", StringGenerator.GenerateString(16)));
        document.Add(WriteValuesParagraph("Antecedentes heredo-familiares: ", StringGenerator.GenerateStringWithSpaces(256)));
        document.Add(WriteValuesParagraph("Antecedentes personales patológicos: ", StringGenerator.GenerateStringWithSpaces(256)));
        document.Add(WriteValuesParagraph("Uso y dependencia del alcohol: ", StringGenerator.GenerateStringWithSpaces(32)));
        document.Add(WriteValuesParagraph("Uso y dependencia del tabaco: ", StringGenerator.GenerateStringWithSpaces(32)));
        document.Add(WriteValuesParagraph("Uso y dependencia de otras sustancias psicoactivas: ", StringGenerator.GenerateStringWithSpaces(32)));
        document.Add(WriteValuesParagraph("Antecedentes personales no patológicos: ", StringGenerator.GenerateStringWithSpaces(256)));
        document.Add(WriteValuesParagraph("Padecimiento actual: ", StringGenerator.GenerateStringWithSpaces(128)));
        //document.Add(WriteValuesParagraph(": ", StringGenerator.GenerateStringWithSpaces()));

        #endregion
        document.Add(new Paragraph());

        #region Phisic exploration
        document.Add(new Paragraph("Exploración física").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph("Interrogatorio", string.Empty));
        document.Add(WriteValuesParagraph("Habitus exterior: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Signos vitales", string.Empty));
        document.Add(WriteValuesParagraph("Temperatura: ", $"{Random.Shared.Next(36, 40)} °C"));
        document.Add(WriteValuesParagraph("Tensión arterial: ", StringGenerator.GenerateStringWithSpaces(256)));
        document.Add(WriteValuesParagraph("Frecuencia cardiaca: ", $"{Random.Shared.Next(36, 40)} pulsaciones/minuto"));
        document.Add(WriteValuesParagraph("Frecuencia respiratoria: ", Random.Shared.Next(20, 40).ToString()));
        document.Add(WriteValuesParagraph("Peso: ", Random.Shared.Next(50, 150).ToString()));
        document.Add(WriteValuesParagraph("Talla: ", Random.Shared.Next(30, 50).ToString()));
        document.Add(WriteValuesParagraph("Descripción de cabeza: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Descripción de cuello: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Descripción de tórax: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Descripción de abdomen: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Descripción de miembros: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Descripción de genitales: ", StringGenerator.GenerateStringWithSpaces(128)));
        document.Add(WriteValuesParagraph("Otros datos: ", StringGenerator.GenerateStringWithSpaces(128)));

        #endregion
        document.Add(new Paragraph());

        #region Previous clinic studies
        document.Add(new Paragraph("Resultados previos y actuales de estudios de laboratorio").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Diagnostics
        document.Add(new Paragraph("Diagnósticos o problemas clínicos").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Pronostics
        document.Add(new Paragraph("Pronóstico").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Therapeutics
        document.Add(new Paragraph("Indicación terapéutica").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Evolution notes
        document.Add(new Paragraph("Notas de evolución").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Clinic square
        document.Add(new Paragraph("Evolución y actualización del cuadro clínico").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Vital signs
        document.Add(new Paragraph("Signos vitales").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(64)));

        #endregion
        document.Add(new Paragraph());

        #region Relevant results
        document.Add(new Paragraph("Resultados relevantes de estudios auxiliares").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Diagnostics and clinical problems
        document.Add(new Paragraph("Diagnósticos y problemas clínicos").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Pronostics
        document.Add(new Paragraph("Pronósticos").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Drugs
        document.Add(new Paragraph("Tratamiento e indicaciones médicas").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Interconsult
        document.Add(new Paragraph("Notas de interconsulta").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Diagnostics criteria
        document.Add(new Paragraph("Criterios diagnósticos").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Studies plan
        document.Add(new Paragraph("Plan de estudios").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        #region Diagnostics and treatment recommendations
        document.Add(new Paragraph("Sugerencias diagnósticas y tratamiento").AddStyle(TitlesStyle));
        document.Add(new Paragraph());
        document.Add(WriteValuesParagraph(string.Empty, StringGenerator.GenerateStringWithSpaces(512)));

        #endregion
        document.Add(new Paragraph());

        return filePath;
    }

    private static Paragraph WriteValuesParagraph(string tag, string text)
    {
        var paragraph = new Paragraph(tag).AddStyle(BoldText);
        paragraph.Add(new Text(text).AddStyle(RegularText));
        return paragraph;
    }
}