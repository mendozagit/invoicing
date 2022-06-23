using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceItem
{
    /// <summary>
    /// Atributo requerido para expresar la clave del producto o del servicio amparado por el presente concepto. Es requerido y deben utilizar las claves del catálogo de productos y servicios, cuando los conceptos que registren por sus actividades correspondan con dichos conceptos.
    /// </summary>
    [XmlAttribute("ClaveProdServ")]
    public string? SatItemId { get; set; }

    /// <summary>
    /// Atributo opcional para expresar el número de parte, identificador del producto o del servicio, la clave de producto o servicio, SKU o equivalente, propia de la operación del emisor, amparado por el presente concepto. Opcionalmente se puede utilizar claves del estándar GTIN.
    /// </summary>
    [XmlAttribute("NoIdentificacion")]
    public string? ItemId { get; set; }


    /// <summary>
    /// Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido por el presente concepto.
    /// </summary>
    [XmlAttribute("Cantidad")]
    public decimal Quantity { get; set; }


    /// <summary>
    /// Atributo requerido para precisar la clave de unidad de medida estandarizada aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
    /// </summary>
    [XmlAttribute("ClaveUnidad")]
    public string? UnitOfMeasureId { get; set; }


    /// <summary>
    /// Atributo opcional para precisar la unidad de medida propia de la operación del emisor, aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
    /// </summary>
    [XmlAttribute("Unidad")]
    public string? UnitOfMeasure { get; set; }

    /// <summary>
    /// Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
    /// </summary>
    [XmlAttribute("Descripcion")]
    public string? Description { get; set; }

    /// <summary>
    /// Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el presente concepto.
    /// </summary>
    [XmlAttribute("ValorUnitario")]
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Importe")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Atributo opcional para representar el importe de los descuentos aplicables al concepto. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Descuento")]
    [DefaultValue(0)]
    public decimal Discount { get; set; }

    /// <summary>
    /// Atributo requerido para expresar si la operación comercial es objeto o no de impuesto.
    /// </summary>
    [XmlAttribute("ObjetoImp")]
    public string? TaxObjectId { get; set; }


    /// <summary>
    /// Nodo condicional para capturar los impuestos aplicables al presente concepto.
    /// </summary>
    [XmlElement("Impuestos")]
    public InvoiceItemTaxesWrapper? ItemTaxex { get; set; }


    #region DomainServices

    /// <summary>
    /// Add transferred tax to current invoice item.
    /// </summary>
    /// <param name="itemTax">transferred tax object</param>
    public void AddTransferredTax(InvoiceItemTax itemTax)
    {
        ItemTaxex ??= new InvoiceItemTaxesWrapper();
        ItemTaxex.TransferredTaxes ??= new List<InvoiceItemTax>();
        ItemTaxex.TransferredTaxes.Add(itemTax);
    }

    /// <summary>
    /// Add transferred tax to current invoice item.
    /// </summary>
    /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
    /// <param name="amount">Importe:Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota.</param>
    public void AddTransferredTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
    {
        ItemTaxex ??= new InvoiceItemTaxesWrapper();
        ItemTaxex.TransferredTaxes ??= new List<InvoiceItemTax>();

        var transferredTax = new InvoiceItemTax
        {
            Base = pbase,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = amount
        };

        ItemTaxex.TransferredTaxes.Add(transferredTax);
    }

    /// <summary>
    /// Add withholding tax to current invoice item.
    /// </summary>
    /// <param name="itemTax">withholding tax object</param>
    public void AddWithholdingTax(InvoiceItemTax itemTax)
    {
        ItemTaxex ??= new InvoiceItemTaxesWrapper();
        ItemTaxex.WithholdingTaxes ??= new List<InvoiceItemTax>();
        ItemTaxex.WithholdingTaxes.Add(itemTax);
    }

    /// <summary>
    /// Add withholding tax to current invoice item.
    /// </summary>
    /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo de la retención, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.</param>
    /// <param name="amount">Importe:Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. No se permiten valores negativos.</param>
    public void AddWithholdingTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
    {
        ItemTaxex ??= new InvoiceItemTaxesWrapper();
        ItemTaxex.WithholdingTaxes ??= new List<InvoiceItemTax>();

        var transferredTax = new InvoiceItemTax
        {
            Base = pbase,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = amount
        };

        ItemTaxex.WithholdingTaxes.Add(transferredTax);
    }

    #endregion
}