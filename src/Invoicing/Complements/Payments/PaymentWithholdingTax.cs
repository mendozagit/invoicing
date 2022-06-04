using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para señalar la información detallada de una retención de impuesto específico conforme al monto del pago recibido.
/// </summary>
public class PaymentWithholdingTax
{
    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto retenido conforme al monto del pago.
    /// </summary>
    [XmlAttribute("ImpuestoP")]
    public string? TaxId { get; set; }


    /// <summary>
    /// Atributo requerido para señalar el importe del impuesto retenido conforme al monto del pago. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("ImporteP")]
    [DefaultValue(0)]
    public decimal Amount { get; set; }
}