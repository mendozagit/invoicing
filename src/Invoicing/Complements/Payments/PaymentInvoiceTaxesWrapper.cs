using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo condicional para registrar los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda del documento relacionado.
/// </summary>
public class PaymentInvoiceTaxesWrapper
{

    /// <summary>
    /// Nodo opcional para capturar los impuestos retenidos aplicables conforme al monto del pago recibido.
    /// </summary>
    [XmlArray("RetencionesDR")]
    [XmlArrayItem("RetencionDR")]
    public List<PaymentInvoiceWithholdingTax>? WithholdingTaxes { get; set; }


    /// <summary>
    /// Nodo opcional para capturar los impuestos trasladados aplicables conforme al monto del pago recibido.
    /// </summary>
    [XmlArray("TrasladosDR")]
    [XmlArrayItem("TrasladoDR")]
    public List<PaymentInvoiceTransferredTax>? InvoiceTransferredTaxes { get; set; }
}