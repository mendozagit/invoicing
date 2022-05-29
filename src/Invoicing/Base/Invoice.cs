using System.Xml.Schema;
using System.Xml.Serialization;
using Invoicing.Common;
using Invoicing.Common.Constants;
using Invoicing.Common.Enums;

namespace Invoicing.Base;

[XmlRoot("Comprobante", Namespace = InvoiceConstants.CurrentNamespace)]
public class Invoice
{
    /// <summary>
    /// Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
    /// </summary>
    [XmlAttribute("Version")]
    public InvoiceVersion InvoiceVersion { get; set; }


    /// <summary>
    /// Atributo opcional para precisar la serie para control interno del contribuyente. Este atributo acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Serie")]
    public string? InvoiceSerie { get; set; }

    /// <summary>
    /// Atributo opcional para control interno del contribuyente que expresa el folio del comprobante, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Folio")]
    public string? InvoiceNuber { get; set; }


    /// <summary>
    /// Atributo requerido para la expresión de la fecha y hora de expedición del Comprobante Fiscal Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora local donde se expide el comprobante.
    /// </summary>
    [XmlAttribute("Fecha")]
    public string? InvoiceDate { get; set; }

    /// <summary>
    /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia las reglas de resolución miscelánea vigente. El sello debe ser expresado como una cadena de texto en formato Base 64.
    /// </summary>
    [XmlAttribute("Sello")]
    public string? SignatureValue { get; set; }


    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante.
    /// </summary>
    [XmlAttribute("FormaPago")]
    public string? PaymentForm { get; set; }

    /// <summary>
    /// Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
    /// </summary>
    [XmlAttribute("NoCertificado")]
    public string? CertificateNumber { get; set; }

    /// <summary>
    /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
    /// </summary>
    [XmlAttribute("Certificado")]
    public string? CertificateB64 { get; set; }


    /// <summary>
    /// Atributo condicional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante atributos o complementos.
    /// </summary>
    [XmlAttribute("CondicionesDePago")]
    public string? PaymentConditions { get; set; }


    /// <summary>
    /// Atributo requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("SubTotal")]
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Atributo condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento.
    /// </summary>
    [XmlAttribute("Descuento")]
    public decimal Discount { get; set; }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.
    /// </summary>
    [XmlAttribute("Moneda")]
    public string? Currency { get; set; }

    /// <summary>
    /// Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
    /// </summary>
    [XmlAttribute("TipoCambio")]
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales y/o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos federales y/o locales. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Total")]
    public decimal Total { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para el contribuyente emisor.
    /// </summary>
    [XmlAttribute("TipoDeComprobante")]
    public InvoiceType InvoiceTypeId { get; set; }


    /// <summary>
    /// Atributo requerido para expresar si el comprobante ampara una operación de exportación.
    /// </summary>
    [XmlAttribute("Exportacion")]
    public string? ExportId { get; set; }


    /// <summary>
    /// Atributo condicional para precisar la clave del método de pago que aplica para este comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción VII incisos a y b del CFF.
    /// </summary>
    [XmlAttribute("MetodoPago")]
    public string? PaymentMethodId { get; set; }


    /// <summary>
    /// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante (domicilio de la matriz o de la sucursal).
    /// </summary>
    [XmlAttribute("LugarExpedicion")]
    public string? ExpeditionZipCode { get; set; }


    /// <summary>
    /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del rango establecido.
    /// </summary>
    [XmlAttribute("Confirmacion")]
    public string? PacConfirmation { get; set; }


    [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
    public string? SchemaLocation { get; set; }


    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    [XmlElement(ElementName = "InformacionGlobal")]
    public InvoiceGlobalInformation? GlobalInformation { get; set; }


    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    [XmlArray(ElementName = "CfdiRelacioandos")]
    [XmlArrayItem(ElementName = "CfdiRelacioando")]
    public List<InvoiceRelated>? InvoiceRelateds { get; set; }


    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    [XmlElement(ElementName = "Emisor")]
    public InvoiceIssuer? InvoiceIssuer { get; set; }


    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    [XmlElement(ElementName = "Receptor")]
    public InvoiceRecipient? InvoiceRecipient { get; set; }

    /// <summary>
    /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
    /// </summary>
    [XmlArray(ElementName = "Conceptos")]
    [XmlArrayItem(ElementName = "Concepto")]
    public List<InvoiceItem>? InvoiceItems { get; set; } = new();


}