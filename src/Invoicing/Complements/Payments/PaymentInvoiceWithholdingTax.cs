using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para registrar la información detallada de una retención de impuesto específico conforme al monto del pago recibido.
/// </summary>
public class PaymentInvoiceWithholdingTax
{
    /// <summary>
    /// Atributo requerido para señalar la base para el cálculo de la retención conforme al monto del pago, aplicable al documento relacionado, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes.No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("BaseDR")]
    [DefaultValue(0)]
    public decimal Base { get; set; }


    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto retenido conforme al monto del pago, aplicable al documento relacionado.
    /// </summary>
    [XmlAttribute("ImpuestoDR")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
    /// </summary>
    [XmlAttribute("TipoFactorDR")]
    public string? TaxTypeId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar el valor de la tasa o cuota del impuesto que se retiene.
    /// </summary>
    [XmlAttribute("TasaOCuotaDR")]
    //[DefaultValue(0)]
    public decimal TaxRate { get; set; }

    /// <summary>
    /// The FooSpecified property is used to control whether the Foo property must be serialized.If you always want to serialize the property, just remove the FooSpecified property.
    /// <see>
    ///     <cref>https://stackoverflow.com/questions/6711906/net-why-must-i-use-specified-property-to-force-serialization-is-there-a-way</cref>
    /// </see>
    /// </summary>
    [XmlIgnore]
    public bool TaxRateSpecified { get; set; } = true;

    /// <summary>
    /// Atributo requerido para señalar el importe del impuesto retenido conforme al monto del pago, aplicable al documento relacionado. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("ImporteDR")]
    //[DefaultValue(0)]
    public decimal Amount { get; set; }

    /// <summary>
    /// The FooSpecified property is used to control whether the Foo property must be serialized.If you always want to serialize the property, just remove the FooSpecified property.
    /// <see>
    ///     <cref>https://stackoverflow.com/questions/6711906/net-why-must-i-use-specified-property-to-force-serialization-is-there-a-way</cref>
    /// </see>
    /// </summary>
    [XmlIgnore]
    public bool AmountSpecified { get; set; } = true;
}