using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo condicional para registrar los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda del documento relacionado.
/// </summary>
public class InvoiceTaxexWrapper
{

    /// <summary>
    /// Nodo opcional para capturar los impuestos retenidos aplicables conforme al monto del pago recibido.
    /// </summary>
    [XmlArray("RetencionesDR")]
    [XmlArrayItem("RetencionDR")]
    public List<InvoiceWithholdingTax>? WithholdingTaxes { get; set; }

    [XmlArray("TrasladosDR")]
    [XmlArrayItem("TraladoDR")]
    public List<InvoiceTransferredTax>? InvoiceTransferredTaxes { get; set; }
}