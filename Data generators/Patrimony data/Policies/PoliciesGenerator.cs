using General_Data;
using Identifying_data.Exterior_numbers;
using Identifying_data.Municipalities;
using Identifying_data.Names;
using Identifying_data.Phone_numbers;
using Identifying_data.Postal_code;
using Identifying_data.Rfc;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TransitAndMigratoryData.Automobile_license_plate;

namespace Patrimony_data.Policies;

public class PoliciesGenerator
{
    private const string PathToExample = "./Policies/PolicyExample.pdf";
    private readonly PdfReader _pdfReader = new(PathToExample);
    private readonly PdfFont _font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
    private readonly Style _text;

    public PoliciesGenerator()
    {
        _text = new Style()
            .SetFont(_font)
            .SetFontSize(8);
    }

    public string GeneratePolicy()
    {
        string fileName = StringGenerator.GenerateString(10),
            filePath = $"./Policies/{fileName}.pdf";
        PdfDocument pdfDocument = new(_pdfReader, new PdfWriter(filePath));
        Document document = new(pdfDocument);

        #region First part
        // Producto
        document.Add(new Paragraph(Random.Shared.Next().ToString()).AddStyle(_text).SetFixedPosition(135f, 680f, 75));
        // Numero de poliza
        document.Add(new Paragraph(Random.Shared.Next().ToString()).AddStyle(_text).SetFixedPosition(190f, 680f, 75));
        // Modulo
        document.Add(new Paragraph(Random.Shared.Next(100).ToString()).AddStyle(_text).SetFixedPosition(260, 680f, 75));
        // Oficina
        document.Add(new Paragraph(Random.Shared.Next(100).ToString()).AddStyle(_text).SetFixedPosition(320, 680f, 75));
        // Ramo
        document.Add(new Paragraph(Random.Shared.Next(100).ToString()).AddStyle(_text).SetFixedPosition(375, 680f, 75));
        // Subramo
        document.Add(new Paragraph(Random.Shared.Next(100).ToString()).AddStyle(_text).SetFixedPosition(430, 680f, 75));
        // Inciso
        document.Add(new Paragraph(((char)Random.Shared.Next(65, 91)).ToString()).AddStyle(_text).SetFixedPosition(490, 680f, 75));
        #endregion

        #region Second part
        // Nombre
        document.Add(new Paragraph(new NamesGenerator().GetRandomName()).AddStyle(_text).SetFixedPosition(165, 605, 200));
        // RFC
        document.Add(new Paragraph(RfcGenerator.Generate()).AddStyle(_text).SetFixedPosition(385, 605, 100));
        // Nombre 2
        document.Add(new Paragraph(new NamesGenerator().GetRandomName()).AddStyle(_text).SetFixedPosition(210, 587, 200));
        // Direccion
        var direction = StreetNamesGenerator.Generate() + " " + ExteriorNumbersGenerator.GenerateHouseNumber();
        document.Add(new Paragraph(direction).AddStyle(_text).SetFixedPosition(145, 568, 250));
        // Colonia
        document.Add(new Paragraph(SettlementsGenerator.Generate()).AddStyle(_text).SetFixedPosition(112, 559, 200));
        // Delegacion o municipio
        document.Add(new Paragraph(MunicipalitiesGenerator.Generate()).AddStyle(_text).SetFixedPosition(375, 559, 200));
        // Codigo postal
        document.Add(new Paragraph(PostalCodeGenerator.Generate()).AddStyle(_text).SetFixedPosition(112, 550, 200));
        // Estado
        document.Add(new Paragraph(StatesGenerator.Generate()).AddStyle(_text).SetFixedPosition(270, 550, 200));
        // Telefono
        document.Add(new Paragraph(PhoneNumbersGenerator.GeneratePhoneNumber().ToString()).AddStyle(_text).SetFixedPosition(395, 550, 200));
        // Conductor
        document.Add(new Paragraph(new NamesGenerator().GetRandomName()).AddStyle(_text).SetFixedPosition(155, 540, 200));
        // Beneficiario pertinente
        document.Add(new Paragraph(new NamesGenerator().GetRandomName()).AddStyle(_text).SetFixedPosition(160, 531, 200));
        #endregion

        #region Tirdth part
        // Tipo de movimiento
        document.Add(new Paragraph(StringGenerator.GenerateString(18)).AddStyle(_text).SetFixedPosition(205, 375, 200));
        // Conducto de cobro
        document.Add(new Paragraph(StringGenerator.GenerateString(18)).AddStyle(_text).SetFixedPosition(205, 359, 200));
        // Intermediario
        document.Add(new Paragraph(StringGenerator.GenerateString(18)).AddStyle(_text).SetFixedPosition(205, 344, 200));
        #endregion

        #region Forth part
        // Descripcion
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(80)).AddStyle(_text).SetFixedPosition(125, 278, 400));
        // Conducto de cobro
        document.Add(new Paragraph(Random.Shared.Next(99999).ToString()).AddStyle(_text).SetFixedPosition(120, 269, 200));
        // Intermediario
        document.Add(new Paragraph(StringGenerator.GenerateString(8)).AddStyle(_text).SetFixedPosition(195, 269, 200));
        // Capacidad
        document.Add(new Paragraph(Random.Shared.Next(99999).ToString()).AddStyle(_text).SetFixedPosition(290, 269, 200));
        // Modelo
        document.Add(new Paragraph(StringGenerator.GenerateString(10)).AddStyle(_text).SetFixedPosition(370, 269, 200));
        // Transmisión
        document.Add(new Paragraph(StringGenerator.GenerateString(10)).AddStyle(_text).SetFixedPosition(470, 269, 200));
        // Categoría
        document.Add(new Paragraph(StringGenerator.GenerateString(40)).AddStyle(_text).SetFixedPosition(120, 258, 400));
        // Uso
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(35)).AddStyle(_text).SetFixedPosition(350, 258, 400));
        // Servicio
        document.Add(new Paragraph(StringGenerator.GenerateString(35)).AddStyle(_text).SetFixedPosition(110, 250, 400));
        // Placa
        document.Add(new Paragraph(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate()).AddStyle(_text).SetFixedPosition(315, 250, 400));
        // Serie
        document.Add(new Paragraph(StringGenerator.GenerateString(10)).AddStyle(_text).SetFixedPosition(425, 250, 400));
        // Repuve
        document.Add(new Paragraph(StringGenerator.GenerateString(25)).AddStyle(_text).SetFixedPosition(110, 240, 400));
        // Tonelaje
        document.Add(new Paragraph(Random.Shared.Next(10).ToString()).AddStyle(_text).SetFixedPosition(270, 240, 200));
        // Motor
        document.Add(new Paragraph(StringGenerator.GenerateString(15)).AddStyle(_text).SetFixedPosition(415, 240, 400));
        // Remolque
        document.Add(new Paragraph(StringGenerator.GenerateString(15)).AddStyle(_text).SetFixedPosition(122, 231, 400));
        // Tipo de remolque
        document.Add(new Paragraph(StringGenerator.GenerateString(15)).AddStyle(_text).SetFixedPosition(281, 231, 400));
        // Tipo de carga
        document.Add(new Paragraph(StringGenerator.GenerateString(20)).AddStyle(_text).SetFixedPosition(408, 231, 400));
        // Descripcion de carga
        document.Add(new Paragraph(StringGenerator.GenerateStringWithSpaces(70)).AddStyle(_text).SetFixedPosition(160, 222, 400));
        // Codigo postal
        document.Add(new Paragraph(PostalCodeGenerator.Generate()).AddStyle(_text).SetFixedPosition(100, 213, 200));
        // Estado
        document.Add(new Paragraph(StatesGenerator.Generate()).AddStyle(_text).SetFixedPosition(270, 213, 200));
        // Telefono
        document.Add(new Paragraph(PhoneNumbersGenerator.GeneratePhoneNumber().ToString()).AddStyle(_text).SetFixedPosition(395, 213, 200));
        // Numero de referencia
        document.Add(new Paragraph(Random.Shared.NextInt64().ToString()).AddStyle(_text).SetFixedPosition(140, 204, 200));
        // Numero de inventario
        document.Add(new Paragraph(Random.Shared.NextInt64().ToString()).AddStyle(_text).SetFixedPosition(140, 194, 200));
        // Numero de pedimento
        document.Add(new Paragraph(Random.Shared.NextInt64().ToString()).AddStyle(_text).SetFixedPosition(360, 194, 200));
        #endregion
        
        document.Close();
        return filePath;
    }
}