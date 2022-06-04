using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Atributo requerido para expresar el identificador del documento relacionado con el pago. Este dato puede ser un Folio Fiscal de la Factura Electrónica o bien el número de operación de un documento digital.
    /// </summary>

    [XmlAttribute("IdDocumento")]
    public string? InvoiceUuid { get; set; }

    /// <summary>
    /// Atributo opcional para precisar la serie del comprobante para control interno del contribuyente, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Serie")]
    public string? InvoiceSeries { get; set; }

    /// <summary>
    /// Atributo opcional para precisar el folio del comprobante para control interno del contribuyente, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Folio")]
    public string? InvoiceNumber { get; set; }


    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada en los importes del documento relacionado, cuando se usa moneda nacional o el documento relacionado no especifica la moneda se registra MXN. Los importes registrados en los atributos “ImpSaldoAnt”, “ImpPagado” e “ImpSaldoInsoluto” de éste nodo, deben corresponder a esta moneda. Conforme con la especificación ISO 4217.
    /// </summary>
    [XmlAttribute("ModenaDR")]
    public string? InvoiceCurrencyId { get; set; }


    /// <summary>
    /// Atributo condicional para expresar el tipo de cambio conforme con la moneda registrada en el documento relacionado. Es requerido cuando la moneda del documento relacionado es distinta de la moneda de pago. Se debe registrar el número de unidades de la moneda señalada en el documento relacionado que equivalen a una unidad de la moneda del pago. Por ejemplo: El documento relacionado se registra en USD. El pago se realiza por 100 EUR. Este atributo se registra como 1.114700 USD/EUR. El importe pagado equivale a 100 EUR * 1.114700 USD/EUR = 111.47 USD.
    /// </summary>
    [XmlAttribute("EquivalenciaDR")]
    public decimal InvoiceExchangeRate { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el número de parcialidad que corresponde al pago.
    /// </summary>
    [XmlAttribute("NumParcialidad")]
    public int PartialityNumber { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el monto del saldo insoluto de la parcialidad anterior. En el caso de que sea la primer parcialidad este atributo debe contener el importe total del documento relacionado.
    /// </summary>
    [XmlAttribute("ImpSaldoAnt")]
    public decimal PreviousBalanceAmount { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el importe pagado para el documento relacionado.
    /// </summary>
    [XmlAttribute("ImpPagado")]
    public decimal PaymentAmount { get; set; }


    /// <summary>
    /// Atributo requerido para expresar la diferencia entre el importe del saldo anterior y el monto del pago.
    /// </summary>
    [XmlAttribute("ImpSaldoInsoluto")]
    public decimal RemainingBalance { get; set; }


    /// <summary>
    /// Atributo requerido para expresar si el pago del documento relacionado es objeto o no de impuesto.
    /// </summary>
    [XmlAttribute("ObjetoImpDR")]
    public string? TaxObjectId { get; set; }

    /// <summary>
    /// Nodo condicional para registrar los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda del documento relacionado.
    /// </summary>
    [XmlElement("ImpuestosDR")]
    public InvoiceTaxesWrapper? InvoiceTaxesWrapper { get; set; }
}