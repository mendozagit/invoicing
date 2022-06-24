using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Base;

public sealed class InvoiceItemTax
{
    /// <summary>
    /// Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Base")]
    public decimal Base { get; set; }


    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.
    /// </summary>
    [XmlAttribute("Impuesto")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
    /// </summary>
    [XmlAttribute("TipoFactor")]
    public string? TaxTypeId { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.
    /// </summary>
    [XmlAttribute("TasaOCuota")]
    [DefaultValue(0)]
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota.
    /// </summary>
    [XmlAttribute("Importe")]
    [DefaultValue(0)]
    public decimal Amount { get; set; }
}