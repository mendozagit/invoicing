using System.Xml.Serialization;

namespace Invoicing.Base;

/// <summary>
/// Nodo requerido para precisar la información de los comprobantes relacionados.
/// </summary>
public sealed class InvoiceRelated
{
    /// <summary>
    /// Atributo requerido para registrar el folio fiscal (UUID) de un CFDI relacionado con el presente comprobante,
    /// Por ejemplo: Si el CFDI relacionado es un comprobante de traslado que sirve para registrar el movimiento de la mercancía.
    /// Si este comprobante se usa como nota de crédito o nota de débito del comprobante relacionado.
    /// Si este comprobante es una devolución sobre el comprobante relacionado.
    /// Si éste sustituye a una factura cancelada.
    /// </summary>
    [XmlAttribute(AttributeName = "UUID")]
    public string? InvoiceUuid { get; set; }
}