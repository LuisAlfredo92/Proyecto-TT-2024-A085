using General_Data;
using Identifying_data.Exterior_numbers;
using Identifying_data.Localities;
using Identifying_data.Municipalities;
using Identifying_data.Names;
using Identifying_data.Postal_code;
using Identifying_data.Rfc;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using Transit_and_migratory_data.Automobile_license_plate;
using Transit_and_migratory_data.Vehicular_license;

namespace Patrimony_data.Taxes;

public class TaxesGenerator
{
    private const string Template = """
                                    <SAPInterfaceXML>
                                      <IDNumber>{0}</IDNumber>
                                      <IDTypeOper>{1}</IDTypeOper>
                                      <SAP>
                                        <AWBNumber>{2}</AWBNumber>
                                        <Origin>OR{3}</Origin>
                                        <Destination>{4}</Destination>
                                        <ShipperName />
                                        <AgentCode>{5}</AgentCode>
                                        <AgentName />
                                        <TotalPieces>{6}</TotalPieces>
                                        <TotalWeight>{7}</TotalWeight>
                                        <ChargedWeight>{8}</ChargedWeight>
                                        <RatePerKG>{9}</RatePerKG>
                                        <Freight>{10}</Freight>
                                        <AWBFees>{11}</AWBFees>
                                        <DomainCharges>{12}</DomainCharges>
                                        <ValuationCharges>{13}</ValuationCharges>
                                        <FuelSurcharge>{14}</FuelSurcharge>
                                        <SecurityCharge>{15}</SecurityCharge>
                                        <MiscOCDC>{16}</MiscOCDC>
                                        <TotalOCDC>{17}</TotalOCDC>
                                        <SubTotal>{18}</SubTotal>
                                        <FreightVATPercentage>{19}</FreightVATPercentage>
                                        <FreightVATAmount>{20}</FreightVATAmount>
                                        <FreightMaterialNo>{21}</FreightMaterialNo>
                                        <OtherChargesVATPercentage>{22}</OtherChargesVATPercentage>
                                        <OtherChargesVATAmount>{23}</OtherChargesVATAmount>
                                        <OtherChargesMaterialNo>{24}</OtherChargesMaterialNo>
                                        <Total>{25}</Total>
                                        <ChargeCode />
                                        <ProductType>{26}</ProductType>
                                        <AWBShipmentType />
                                        <UOM>{27}</UOM>
                                        <Currency />
                                        <CommodityCode />
                                        <FirstFlightOrigin />
                                        <FirstFlightDestination />
                                        <FirstFlightCarrier />
                                        <FirstFlightNumber />
                                        <FirstFlightDate />
                                        <SAPZCode />
                                        <WeightCCA>{28}</WeightCCA>
                                        <RateCCA>{29}</RateCCA>
                                        <FreightCCA>{30}</FreightCCA>
                                        <FreightVATPercentageCCA>{31}</FreightVATPercentageCCA>
                                        <FreightVATAmountCCA>{32}</FreightVATAmountCCA>
                                        <TotalOCDCCCA>{33}</TotalOCDCCCA>
                                        <OtherChargesVATPercentageCCA>{34}</OtherChargesVATPercentageCCA>
                                        <OtherChargesVATAmountCCA>{35}</OtherChargesVATAmountCCA>
                                        <TotalCCA>{36}</TotalCCA>
                                        <DateCCA>{37}</DateCCA>
                                        <DeliveryDate />
                                        <SAPStatus />
                                        <InvoiceNumber />
                                        <UUID />
                                      </SAP>
                                      <cfdi:Complemento>
                                        <cartaporte20:CartaPorte Version="2.0" TranspInternac="No" TotalDistRec="{38}">
                                          <cartaporte20:Ubicaciones>
                                            <cartaporte20:Ubicacion TipoUbicacion="Origen" IDUbicacion="OR{39}" RFCRemitenteDestinatario="{40}" NombreRemitenteDestinatario="{41}" NumRegIdTrib="{42}" FechaHoraSalidaLlegada="{43}" />
                                            <cartaporte20:Ubicacion TipoUbicacion="Destino" IDUbicacion="DE{44}" RFCRemitenteDestinatario="{45}" NombreRemitenteDestinatario="{46}" NumRegIdTrib="{47}" FechaHoraSalidaLlegada="{48}" DistanciaRecorrida="{49}" />
                                          </cartaporte20:Ubicaciones>
                                          <cartaporte20:Mercancias PesoBrutoTotal="{50}" UnidadPeso="{51}" NumTotalMercancias="{52}">
                                            <cartaporte20:Mercancia BienesTransp="{53}" ClaveSTCC="{54}" Descripcion="{55}" Cantidad="{56}" ClaveUnidad="{57}" Unidad="{58}" MaterialPeligroso="{59}" CveMaterialPeligroso="{60}" Embalaje="{61}" PesoEnKg="{62}" ValorMercancia="{63}" Moneda="{64}" />
                                            <cartaporte20:Autotransporte PermSCT="{65}" NumPermisoSCT="{66}">
                                              <cartaporte20:IdentificacionVehicular ConfigVehicular="{67}" PlacaVM="{68}" AnioModeloVM="{69}" />
                                            </cartaporte20:Autotransporte>
                                            <cartaporte20:TransporteAereo PermSCT="{70}" NumPermisoSCT="{71}" MatriculaAeronave="{72}" NumeroGuia="{73}" CodigoTransportista="{74}" />
                                          </cartaporte20:Mercancias>
                                          <cartaporte20:FiguraTransporte>
                                            <cartaporte20:TiposFigura TipoFigura="{75}" RFCFigura="{76}" NumLicencia="{77}" NombreFigura="{78}">
                                              <cartaporte20:PartesTransporte>
                                                <cartaporte20:Domicilio Calle="{79}" NumeroExterior="{80}" Colonia="{81}" Localidad="{82}" Municipio="{83}" Estado="{84}" Pais="MEX" CodigoPostal="{85}" />
                                              </cartaporte20:PartesTransporte>
                                            </cartaporte20:TiposFigura>
                                            <cartaporte20:TiposFigura TipoFigura="{86}" RFCFigura="{87}" NombreFigura="{88}" />
                                          </cartaporte20:FiguraTransporte>
                                        </cartaporte20:CartaPorte>
                                      </cfdi:Complemento>
                                    </SAPInterfaceXML>
                                    """;

    public static string GenerateTaxes()
    {
        return string.Format(Template,
            Random.Shared.Next(), // 0
            Random.Shared.Next(), // 1
            Random.Shared.Next(999), // 2
            Random.Shared.Next(999999).ToString().PadLeft(6,'0'), // 3
            StringGenerator.GenerateString(3), // 4
            Random.Shared.Next(), // 5
            Random.Shared.Next(), // 6
            Random.Shared.Next(), // 7
            Random.Shared.Next(), // 8
            Random.Shared.Next(), // 9
            Random.Shared.Next(), // 10
            Random.Shared.Next(), // 11
            Random.Shared.Next(), // 12
            Random.Shared.Next(), // 13
            Random.Shared.Next(), // 14
            Random.Shared.Next(), // 15
            Random.Shared.Next(), // 16
            Random.Shared.Next(), // 17
            Random.Shared.Next(), // 18
            Random.Shared.Next(), // 19
            Random.Shared.Next(), // 20
            Random.Shared.Next(), // 21
            Random.Shared.Next(), // 22
            Random.Shared.Next(), // 23
            Random.Shared.Next(), // 24
            Random.Shared.Next(), // 25
            StringGenerator.GenerateString(3), // 26
            StringGenerator.GenerateString(3), // 27
            Random.Shared.Next(), // 28
            Random.Shared.Next(), // 29
            Random.Shared.Next(), // 30
            Random.Shared.Next(), // 31
            Random.Shared.Next(), // 32
            Random.Shared.Next(), // 33
            Random.Shared.Next(), // 34
            Random.Shared.Next(), // 35
            Random.Shared.Next(), // 36
            DatesGenerator.GenerateDate(DateTime.Now, DateTime.MinValue), // 37
            Random.Shared.Next(), // 38
            Random.Shared.Next(999999).ToString().PadLeft(6, '0'), // 39
            RfcGenerator.Generate(), // 40
            StringGenerator.GenerateStringWithSpaces(50), // 41
            Random.Shared.Next(), // 42
            DatesGenerator.GenerateDate(DateTime.Now, DateTime.MinValue), // 43
            Random.Shared.Next(999999).ToString().PadLeft(6, '0'), // 44
            RfcGenerator.Generate(), // 45
            StringGenerator.GenerateStringWithSpaces(50), // 46
            Random.Shared.Next(), // 47
            DatesGenerator.GenerateDate(DateTime.Now, DateTime.MinValue), // 48
            Random.Shared.Next(), // 49
            Random.Shared.Next(), // 50
            StringGenerator.GenerateString(3), // 51
            Random.Shared.Next(), // 52
            Random.Shared.Next(), // 53
            Random.Shared.Next(), // 54
            StringGenerator.GenerateStringWithSpaces(32), // 55
            Random.Shared.Next(), // 56
            StringGenerator.GenerateString(3), // 57
            StringGenerator.GenerateString(3), // 58
            Random.Shared.NextSingle() < 0.5f ? "Sí" : "No", // 59
            Random.Shared.Next(), // 60
            StringGenerator.GenerateStringWithNumbers(2), // 61
            Random.Shared.Next(), // 62
            Random.Shared.Next(), // 63
            StringGenerator.GenerateString(3), // 64
            StringGenerator.GenerateStringWithNumbers(6), // 65
            StringGenerator.GenerateStringWithNumbers(20), // 66
            StringGenerator.GenerateStringWithNumbers(2), // 67
            AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate(), // 68
            Random.Shared.Next(2010, 2025), // 69
            StringGenerator.GenerateStringWithNumbers(6), // 70
            StringGenerator.GenerateStringWithNumbers(20), // 71
            AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate(), // 72
            StringGenerator.GenerateStringWithNumbers(12), // 73
            StringGenerator.GenerateStringWithNumbers(6), // 74
            Random.Shared.Next(), // 75
            RfcGenerator.Generate(), // 76
            VehicularLicenseGenerator.GenerateVehicularLicense(), // 77
            NamesGenerator.Generate(), // 78
            StreetNamesGenerator.Generate(), // 79
            ExteriorNumbersGenerator.GenerateHouseNumber(), // 80
            SettlementsGenerator.Generate(), // 81
            LocalitiesGenerator.Generate(), // 82
            MunicipalitiesGenerator.Generate(), // 83
            StatesGenerator.Generate(), // 84
            PostalCodeGenerator.Generate(), // 85
            Random.Shared.Next(), // 86
            RfcGenerator.Generate(), // 87
            NamesGenerator.Generate() // 88
            );
    }
}