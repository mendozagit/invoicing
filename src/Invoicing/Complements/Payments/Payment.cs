using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

public class Payment
{
    /// <summary>
    /// Atributo requerido para expresar la fecha y hora en la que el beneficiario recibe el pago. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.En caso de no contar con la hora se debe registrar 12:00:00.
    /// </summary>
    [XmlAttribute("FechaPago")]
    public string? PaymentDate { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave de la forma en que se realiza el pago.
    /// </summary>
    [XmlAttribute("FormaDePagoP")]
    public string? PaymentFormId { get; set; }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para realizar el pago conforme a la especificación ISO 4217. Cuando se usa moneda nacional se registra MXN. El atributo Pagos:Pago:Monto debe ser expresado en la moneda registrada en este atributo.
    /// </summary>
    [XmlAttribute("MonedaP")]
    public string? CurrencyId { get; set; }


    /// <summary>
    /// Atributo condicional para expresar el tipo de cambio de la moneda a la fecha en que se realizó el pago. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo MonedaP. Es requerido cuando el atributo MonedaP es diferente a MXN.
    /// </summary>
    [XmlAttribute("TipoCambioP")]
    [DefaultValue(0)]
    public decimal ExchangeRate { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el importe del pago.
    /// </summary>
    [XmlAttribute("Monto")]
    public decimal Ammount { get; set; }

    /// <summary>
    /// Atributo condicional para expresar el número de cheque, número de autorización, número de referencia, clave de rastreo en caso de ser SPEI, línea de captura o algún número de referencia análogo que identifique la operación que ampara el pago efectuado.
    /// </summary>
    [XmlAttribute("NumOperacion")]
    public string? OperationNumber { get; set; }


    //Origin and destination

    /// <summary>
    /// Atributo condicional para expresar la clave RFC de la entidad emisora de la cuenta origen, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc., en caso de ser extranjero colocar XEXX010101000, considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("RfcEmisorCtaOrd")]
    public string? OriginBankTin { get; set; }


    /// <summary>
    /// Atributo condicional para incorporar el número de la cuenta con la que se realizó el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("CtaOrdenante")]
    public string? OriginBankAccountNumber { get; set; }

    /// <summary>
    /// Atributo condicional para expresar la clave RFC de la entidad operadora de la cuenta destino, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("RfcEmisorCtaBen")]
    public string? DestinationBankTin { get; set; }

    /// <summary>
    /// Atributo condicional para incorporar el número de cuenta en donde se recibió el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("CtaBeneficiario")]
    public string? DestinationAccountNumber { get; set; }

    /// <summary>
    /// Atributo condicional para expresar el nombre del banco ordenante, es requerido en caso de ser extranjero. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("NomBancoOrdExt")]
    public string? ForeignBankName { get; set; }


    /// <summary>
    /// Atributo condicional para identificar la clave del tipo de cadena de pago que genera la entidad receptora del pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    /// <value>01-SPEI</value>
    [XmlAttribute("TipoCadPago")]
    public string? ElectronicPaymentSystemId { get; set; }

    /// <summary>
    /// Atributo condicional que sirve para incorporar el certificado que ampara al pago, como una cadena de texto en formato base 64. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("CertPago")]
    public string? Base64PaymetCertificate { get; set; }

    /// <summary>
    /// Atributo condicional para expresar la cadena original del comprobante de pago generado por la entidad emisora de la cuenta beneficiaria. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("CadPago")]
    public string? PaymentOriginalString { get; set; }


    /// <summary>
    /// Atributo condicional para integrar el sello digital que se asocie al pago. La entidad que emite el comprobante de pago, ingresa una cadena original y el sello digital en una sección de dicho comprobante, este sello digital es el que se debe registrar en este atributo. Debe ser expresado como una cadena de texto en formato base 64. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("SelloPago")]
    public string? SignatureValue { get; set; }


    /// <summary>
    /// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
    /// </summary>
    [XmlArray(ElementName = "DoctoRelacionado")]
    [XmlArrayItem(ElementName = "DoctoRelacionado")]
    public List<PaymentInvoice>? Invoices { get; set; }


    /// <summary>
    /// Nodo condicional para registrar el resumen de los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda de pago.
    /// </summary>
    [XmlElement("ImpuestosP")]
    public PaymentTaxesWrapper? PaymentTaxexWrapper { get; set; }
}