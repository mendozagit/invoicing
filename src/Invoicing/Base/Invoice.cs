using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;

namespace Invoicing.Base;

[XmlRoot("Comprobante", Namespace = InvoiceConstants.SatInvoice40Namespace)]
public sealed class Invoice : ComputeSettings, IComputable, IInvoice
{
    /// <summary>
    /// Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
    /// </summary>
    [XmlAttribute("Version")]
    public InvoiceVersion InvoiceVersion { get; set; }


    /// <summary>
    /// Atributo opcional para precisar la serie para control interno del contribuyente. Este atributo acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Serie")]
    public string? InvoiceSerie { get; set; }

    /// <summary>
    /// Atributo opcional para control interno del contribuyente que expresa el folio del comprobante, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Folio")]
    public string? InvoiceNuber { get; set; }


    /// <summary>
    /// Atributo requerido para la expresión de la fecha y hora de expedición del Comprobante Fiscal Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora local donde se expide el comprobante.
    /// </summary>
    [XmlAttribute("Fecha")]
    public string? InvoiceDate { get; set; }

    /// <summary>
    /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia las reglas de resolución miscelánea vigente. El sello debe ser expresado como una cadena de texto en formato Base 64.
    /// </summary>
    [XmlAttribute("Sello")]
    public string? SignatureValue { get; set; }


    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante.
    /// </summary>
    [XmlAttribute("FormaPago")]
    public string? PaymentForm { get; set; }

    /// <summary>
    /// Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
    /// </summary>
    [XmlAttribute("NoCertificado")]
    public string? CertificateNumber { get; set; }

    /// <summary>
    /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
    /// </summary>
    [XmlAttribute("Certificado")]
    public string? CertificateB64 { get; set; }


    /// <summary>
    /// Atributo condicional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante atributos o complementos.
    /// </summary>
    [XmlAttribute("CondicionesDePago")]
    public string? PaymentConditions { get; set; }


    /// <summary>
    /// Atributo requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("SubTotal")]
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Atributo condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento.
    /// </summary>
    [XmlAttribute("Descuento")]
    [DefaultValue(0)]
    public decimal Discount { get; set; }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.
    /// </summary>
    [XmlAttribute("Moneda")]
    public string? Currency { get; set; }

    /// <summary>
    /// Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
    /// </summary>
    [XmlAttribute("TipoCambio")]
    [DefaultValue(0)]
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales y/o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos federales y/o locales. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
    /// </summary>
    [XmlAttribute("Total")]
    public decimal Total { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para el contribuyente emisor.
    /// </summary>
    [XmlAttribute("TipoDeComprobante")]
    public InvoiceType InvoiceTypeId { get; set; }


    /// <summary>
    /// Atributo requerido para expresar si el comprobante ampara una operación de exportación.
    /// </summary>
    [XmlAttribute("Exportacion")]
    public string? ExportId { get; set; }


    /// <summary>
    /// Atributo condicional para precisar la clave del método de pago que aplica para este comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción VII incisos a y b del CFF.
    /// </summary>
    [XmlAttribute("MetodoPago")]
    public string? PaymentMethodId { get; set; }


    /// <summary>
    /// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante (domicilio de la matriz o de la sucursal).
    /// </summary>
    [XmlAttribute("LugarExpedicion")]
    public string? ExpeditionZipCode { get; set; }


    /// <summary>
    /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del rango establecido.
    /// </summary>
    [XmlAttribute("Confirmacion")]
    public string? PacConfirmation { get; set; }


    [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
    public string? SchemaLocation { get; set; }


    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    [XmlElement(ElementName = "InformacionGlobal")]
    public InvoiceGlobalInformation? GlobalInformation { get; set; }


    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    [XmlElement(ElementName = "CfdiRelacionados")]
    public InvoiceRelatedWrapper? RelatedInvoiceWrapper { get; set; }


    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    [XmlElement(ElementName = "Emisor")]
    public InvoiceIssuer? InvoiceIssuer { get; set; }


    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    [XmlElement(ElementName = "Receptor")]
    public InvoiceRecipient? InvoiceRecipient { get; set; }

    /// <summary>
    /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
    /// </summary>
    [XmlArray(ElementName = "Conceptos")]
    [XmlArrayItem(ElementName = "Concepto")]
    public List<InvoiceItem> InvoiceItems { get; set; } = new();


    /// <summary>
    /// Nodo condicional para expresar el resumen de los impuestos aplicables a la factura.
    /// </summary>
    [XmlElement("Impuestos")]
    public InvoiceTaxesWrapper? InvoiceTaxes { get; set; } = new();


    /// <summary>
    /// Nodo opcional donde se incluye el complemento Timbre Fiscal Digital de manera obligatoria y los nodos complementarios determinados por el SAT, de acuerdo con las disposiciones particulares para un sector o actividad específica.
    /// </summary>
    [XmlElement("Complemento")]
    public List<XElement>? Complements { get; set; }

    /// <summary>
    /// Helper property to serialize applicable supplements after stamping 
    /// </summary>
    [XmlIgnore]
    public List<InvoiceDeserializedComplement>? DeserializedComplements { get; set; }

    #region Helpers

    /// <summary>
    /// This method makes the calculation of all invoice values, it is recommended to use it instead of doing the calculations yourself.
    /// If you use this method, make sure that the amounts generated by the ComputeInvoice() method are the same as the ones you did internally, this ensures that the administrative records in your systems are consistent with the fiscal records in the SAT.
    /// </summary>
    private void ComputeInvoiceInCascade()
    {
        var transferredTaxes = new List<InvoiceItemTax>();
        var withholdingTaxes = new List<InvoiceItemTax>();

        foreach (var invoiceItem in InvoiceItems)
        {
            invoiceItem.Amount =
                Math.Round(invoiceItem.Quantity * invoiceItem.UnitCost, ItemsDecimals, RoundingStrategy);


            if (invoiceItem?.ItemTaxex?.TransferredTaxes is not null)
            {
                foreach (var transferredTax in invoiceItem.ItemTaxex.TransferredTaxes.ToList())
                {
                    transferredTax.Base = Math.Round(invoiceItem.Amount, ItemsDecimals, RoundingStrategy);

                    transferredTax.Amount =
                        Math.Round(transferredTax.Base * transferredTax.TaxRate, ItemsDecimals, RoundingStrategy);


                    transferredTaxes.Add(transferredTax);
                }
            }


            if (invoiceItem?.ItemTaxex?.WithholdingTaxes is null) continue;
            foreach (var withholdingTax in invoiceItem.ItemTaxex.WithholdingTaxes.ToList())
            {
                withholdingTax.Base = Math.Round(invoiceItem.Amount, ItemsDecimals, RoundingStrategy);

                withholdingTax.Amount =
                    Math.Round(withholdingTax.Base * withholdingTax.TaxRate, ItemsDecimals, RoundingStrategy);

                withholdingTaxes.Add(withholdingTax);
            }
        }

        ComputeHeader(transferredTaxes, withholdingTaxes);
    }

    private void ComputeHeader(List<InvoiceItemTax> transferredTaxes, List<InvoiceItemTax> withholdingTaxes)
    {
        #region Summarize Transferred Taxes

        //Agrupar traslados por:  Impuesto, TasaOCuota, TipoFactor
        var grupedTransferredTaxes = transferredTaxes.GroupBy(item => new { item.TaxId, item.TaxRate, item.TaxTypeId })
            .Select(g => new InvoiceTransferredTax()
            {
                TaxId = g.Key.TaxId,
                TaxRate = g.Key.TaxRate,
                TaxTypeId = g.Key.TaxTypeId,
                Base = g.Sum(x => x.Base),
                Amount = g.Sum(x => x.Amount),
            });


        var groupedTransferredTaxes = grupedTransferredTaxes.ToList();
        foreach (var grupedTransferredTax in groupedTransferredTaxes)
        {
            InvoiceTaxes ??= new InvoiceTaxesWrapper();

            InvoiceTaxes.TransferredTaxes ??= new List<InvoiceTransferredTax>();

            InvoiceTaxes.TransferredTaxes?.Add(new InvoiceTransferredTax()
            {
                TaxId = grupedTransferredTax.TaxId,
                TaxTypeId = grupedTransferredTax.TaxTypeId,
                TaxRate = grupedTransferredTax.TaxRate,
                Base = Math.Round(grupedTransferredTax.Base, HeaderDecimals, RoundingStrategy),
                Amount = Math.Round(grupedTransferredTax.Amount, HeaderDecimals, RoundingStrategy)
            });
        }

        InvoiceTaxes ??= new InvoiceTaxesWrapper();
        InvoiceTaxes.TotalTransferredTaxes =
            Math.Round(groupedTransferredTaxes.Where(x => x.TaxRate > 0).Sum(x => x.Amount), HeaderDecimals,
                RoundingStrategy);

        #endregion


        #region Summarize Withholding Taxes

        //Agrupar retenciones por:  Impuesto, TasaOCuota, TipoFactor
        var grupedWithholdingTaxes = withholdingTaxes.GroupBy(item => new { item.TaxId, item.TaxRate, item.TaxTypeId })
            .Select(g => new InvoiceTransferredTax()
            {
                TaxId = g.Key.TaxId,
                TaxRate = g.Key.TaxRate,
                TaxTypeId = g.Key.TaxTypeId,
                Base = g.Sum(x => x.Base),
                Amount = g.Sum(x => x.Amount),
            });


        var groupedWithholdingsTaxes = grupedWithholdingTaxes.ToList();
        foreach (var grupedWithholdingTax in groupedWithholdingsTaxes)
        {
            InvoiceTaxes ??= new InvoiceTaxesWrapper();
            InvoiceTaxes.WithholdingTaxes ??= new List<InvoiceWithholdingTax>();
            InvoiceTaxes.WithholdingTaxes?.Add(new InvoiceWithholdingTax()
            {
                TaxId = grupedWithholdingTax.TaxId,
                Amount = Math.Round(grupedWithholdingTax.Amount, HeaderDecimals, RoundingStrategy)
            });
        }

        InvoiceTaxes ??= new InvoiceTaxesWrapper();
        InvoiceTaxes.TotalWithholdingTaxes =
            Math.Round(groupedWithholdingsTaxes.Where(x => x.TaxRate > 0).Sum(x => x.Amount), HeaderDecimals,
                RoundingStrategy);

        #endregion


        #region Summary Totals

        Discount = Math.Round(InvoiceItems.Select(x => x.Discount).Sum(), HeaderDecimals, RoundingStrategy);

        Subtotal = Math.Round(InvoiceItems.Select(x => x.Amount).Sum(), HeaderDecimals, RoundingStrategy);
        Total = Math.Round(
            Subtotal - Discount + InvoiceTaxes.TotalTransferredTaxes - InvoiceTaxes.TotalWithholdingTaxes,
            HeaderDecimals,
            RoundingStrategy); //subtotal-descuentos+ impuestos trasladados -impuestos retenidos

        #endregion
    }


    public void AddComplement(XElement? element)
    {
        Complements ??= new List<XElement>();

        if (element is null)
            throw new ArgumentNullException(nameof(element), "The XmlElement invoice complement cannot be null");


        Complements.Add(element);
    }

    #endregion

    public void Compute()
    {
        ComputeInvoiceInCascade();
        RemoveUnnecessaryElements();
    }

    /// <summary>
    /// Set properties to null based on invoice type and sat rules.
    /// When setting null or the default value, the serializer will skip serialization instructions.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Does not exit invoice type</exception>
    private void RemoveUnnecessaryElements()
    {
        switch (InvoiceTypeId)
        {
            case InvoiceType.Ingreso:
                RemoveUnnecessaryElementsWhenRevenue();
                break;
            case InvoiceType.Egreso:
                RemoveUnnecessaryElementsWhenCreditNote();
                break;
            case InvoiceType.Traslado:
                RemoveUnnecessaryElementsWhenWaybill();
                break;
            case InvoiceType.Nomina:
                RemoveUnnecessaryElementsWhenPayroll();
                break;
            case InvoiceType.Pago:
                RemoveUnnecessaryElementsWhenPayment();
                break;
            default:
                throw new NotSupportedException("Invoice type not yet implemented.");
        }
    }

    private void RemoveUnnecessaryElementsWhenRevenue()
    {
    }

    private void RemoveUnnecessaryElementsWhenCreditNote()
    {
    }

    private void RemoveUnnecessaryElementsWhenWaybill()
    {
        throw new NotImplementedException();
    }

    private void RemoveUnnecessaryElementsWhenPayroll()
    {
        throw new NotImplementedException();
    }

    private void RemoveUnnecessaryElementsWhenPayment()
    {
        InvoiceTaxes = null;
    }
}