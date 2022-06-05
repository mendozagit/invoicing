using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para asentar la información detallada de un traslado de impuesto específico conforme al monto del pago recibido.
/// </summary>
public class PaymentInvoiceTransferredTax
{
    /// <summary>
    /// Atributo requerido para señalar la base para el cálculo del impuesto trasladado conforme al monto del pago, aplicable al documento relacionado, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("BaseDR")]
    [DefaultValue(0)]
    public decimal Base { get; set; }


    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto trasladado conforme al monto del pago, aplicable al documento relacionado
    /// </summary>
    [XmlAttribute("ImpuestoDR")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
    /// </summary>
    [XmlAttribute("TipoFactorDR")]
    public string? TaxTypeId { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada. Es requerido cuando el atributo TipoFactorDR contenga una clave que corresponda a Tasa o Cuota.
    /// </summary>
    [XmlAttribute("TasaOCuotaDR")]
    [DefaultValue(0)]
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el importe del impuesto trasladado conforme al monto del pago, aplicable al documento relacionado. No se permiten valores negativos. Es requerido cuando el tipo factor sea Tasa o Cuota
    /// </summary>
    [XmlAttribute("ImporteDR")]
    [DefaultValue(0)]
    public decimal Amount { get; set; }
}