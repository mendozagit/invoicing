using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para señalar la información detallada de un traslado de impuesto específico conforme al monto del pago recibido.
/// </summary>
public class PaymentTransferredTax
{
    /// <summary>
    /// Atributo requerido para señalar la suma de los atributos BaseDR de los documentos relacionados del impuesto trasladado. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("BaseP")]
    [DefaultValue(0)]
    public decimal Base { get; set; }


    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto trasladado conforme al monto del pago.
    /// </summary>
    [XmlAttribute("ImpuestoP")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
    /// </summary>
    [XmlAttribute("TipoFactorP")]
    public string? TaxTypeId { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada en los documentos relacionados.
    /// </summary>
    [XmlAttribute("TasaOCuotaP")]
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
    /// Atributo condicional para señalar la suma del impuesto trasladado, agrupado por ImpuestoP, TipoFactorP y TasaOCuotaP. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("ImporteP")]
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