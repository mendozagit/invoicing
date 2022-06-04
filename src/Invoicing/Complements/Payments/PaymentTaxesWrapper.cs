using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

public class PaymentTaxesWrapper
{
    /// <summary>
    /// Nodo condicional para señalar los impuestos retenidos aplicables conforme al monto del pago recibido. Es requerido cuando en los documentos relacionados se registre algún impuesto retenido.
    /// </summary>
    [XmlArray("RetencionesP")]
    [XmlArrayItem("RetencionP")]
    public List<PaymentWithholdingTax>? PaymentWithholdingTaxes { get; set; }


    /// <summary>
    /// Nodo condicional para capturar los impuestos trasladados aplicables conforme al monto del pago recibido. Es requerido cuando en los documentos relacionados se registre un impuesto trasladado.
    /// </summary>
    [XmlArray("TrasladosP")]
    [XmlArrayItem("TrasladoP")]
    public List<PaymentTransferredTax>? PaymentTransferredTaxes { get; set; }
}