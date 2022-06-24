using System.Xml.Linq;
using Invoicing.Base;
using Invoicing.Common.Enums;

namespace Invoicing.Common.Contracts;

internal interface IInvoice
{
    /// <summary>
    /// Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
    /// </summary>
    InvoiceVersion InvoiceVersion { get; set; }

    /// <summary>
    /// Atributo opcional para precisar la serie para control interno del contribuyente. Este atributo acepta una cadena de caracteres.
    /// </summary>
    string? InvoiceSerie { get; set; }

    /// <summary>
    /// Atributo opcional para control interno del contribuyente que expresa el folio del comprobante, acepta una cadena de caracteres.
    /// </summary>
    string? InvoiceNuber { get; set; }

    /// <summary>
    /// Atributo requerido para la expresión de la fecha y hora de expedición del Comprobante Fiscal Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora local donde se expide el comprobante.
    /// </summary>
    string? InvoiceDate { get; set; }

    /// <summary>
    /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia las reglas de resolución miscelánea vigente. El sello debe ser expresado como una cadena de texto en formato Base 64.
    /// </summary>
    string? SignatureValue { get; set; }

    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante.
    /// </summary>
    string? PaymentForm { get; set; }

    /// <summary>
    /// Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
    /// </summary>
    string? CertificateNumber { get; set; }

    /// <summary>
    /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
    /// </summary>
    string? CertificateB64 { get; set; }

    /// <summary>
    /// Atributo condicional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante atributos o complementos.
    /// </summary>
    string? PaymentConditions { get; set; }

    /// <summary>
    /// Atributo requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
    /// </summary>
    decimal Subtotal { get; set; }

    /// <summary>
    /// Atributo condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento.
    /// </summary>
    decimal Discount { get; set; }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.
    /// </summary>
    string? Currency { get; set; }

    /// <summary>
    /// Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
    /// </summary>
    decimal ExchangeRate { get; set; }

    /// <summary>
    /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales y/o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos federales y/o locales. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
    /// </summary>
    decimal Total { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para el contribuyente emisor.
    /// </summary>
    InvoiceType InvoiceTypeId { get; set; }

    /// <summary>
    /// Atributo requerido para expresar si el comprobante ampara una operación de exportación.
    /// </summary>
    string? ExportId { get; set; }

    /// <summary>
    /// Atributo condicional para precisar la clave del método de pago que aplica para este comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción VII incisos a y b del CFF.
    /// </summary>
    string? PaymentMethodId { get; set; }

    /// <summary>
    /// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante (domicilio de la matriz o de la sucursal).
    /// </summary>
    string? ExpeditionZipCode { get; set; }

    /// <summary>
    /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del rango establecido.
    /// </summary>
    string? PacConfirmation { get; set; }

    string? SchemaLocation { get; set; }

    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    InvoiceGlobalInformation? GlobalInformation { get; set; }

    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    InvoiceRelatedWrapper? RelatedInvoiceWrapper { get; set; }

    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    InvoiceIssuer? InvoiceIssuer { get; set; }

    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    InvoiceRecipient? InvoiceRecipient { get; set; }

    /// <summary>
    /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
    /// </summary>
    List<InvoiceItem> InvoiceItems { get; set; }

    /// <summary>
    /// Nodo condicional para expresar el resumen de los impuestos aplicables a la factura.
    /// </summary>
    InvoiceTaxesWrapper? InvoiceTaxes { get; set; }

    /// <summary>
    /// Nodo opcional donde se incluye el complemento Timbre Fiscal Digital de manera obligatoria y los nodos complementarios determinados por el SAT, de acuerdo con las disposiciones particulares para un sector o actividad específica.
    /// </summary>
    List<XElement>? Complements { get; set; }

    /// <summary>
    /// Helper property to serialize applicable supplements after stamping 
    /// </summary>
    List<InvoiceDeserializedComplement>? DeserializedComplements { get; set; }

    /// <summary>
    /// Number of decimal places in header fields
    /// </summary>
    int HeaderDecimals { get; set; }

    /// <summary>
    /// Number of decimal places in items fields
    /// </summary>
    int ItemsDecimals { get; set; }

    /// <summary>
    /// Rounding strategy used in invoice computation.
    /// </summary>
    MidpointRounding RoundingStrategy { get; set; }

    void AddComplement(XElement? element);
    void Compute();
}