using System.ComponentModel;
using System.Security.AccessControl;
using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceTax
{
    /// <summary>
    /// Atributo requerido para señalar la suma de los atributos Base de los conceptos del impuesto trasladado. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Base")]
    [DefaultValue(0)]
    public decimal Base { get; set; }


    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de impuesto trasladado.
    /// </summary>
    [XmlAttribute("Impuesto")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
    /// </summary>
    [XmlAttribute("TipoFactor")]
    public string? TaxTypeId { get; set; }

    /// <summary>
    /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada por los conceptos amparados en el comprobante.
    /// </summary>
    [XmlAttribute("TasaOCuota")]
    [DefaultValue(0)]
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Atributo condicional para señalar la suma del importe del impuesto trasladado, agrupado por impuesto, TipoFactor y TasaOCuota. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Importe")]
    [DefaultValue(0)]
    public decimal Amount { get; set; }
}