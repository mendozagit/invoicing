using System.Xml.Serialization;
using Invoicing.Base;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
/// </summary>
public class PaymentInvoice
{
    /// <summary>
    /// Atributo requerido para expresar el identificador del documento relacionado con el pago. Este dato puede ser un Folio Fiscal de la Factura Electrónica o bien el número de operación de un documento digital.
    /// </summary>

    [XmlAttribute("IdDocumento")]
    public string? InvoiceUuid { get; set; }

    /// <summary>
    /// Atributo opcional para precisar la serie del comprobante para control interno del contribuyente, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Serie")]
    public string? InvoiceSeries { get; set; }

    /// <summary>
    /// Atributo opcional para precisar el folio del comprobante para control interno del contribuyente, acepta una cadena de caracteres.
    /// </summary>
    [XmlAttribute("Folio")]
    public string? InvoiceNumber { get; set; }


    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada en los importes del documento relacionado, cuando se usa moneda nacional o el documento relacionado no especifica la moneda se registra MXN. Los importes registrados en los atributos “ImpSaldoAnt”, “ImpPagado” e “ImpSaldoInsoluto” de éste nodo, deben corresponder a esta moneda. Conforme con la especificación ISO 4217.
    /// </summary>
    [XmlAttribute("MonedaDR")]
    public string? InvoiceCurrencyId { get; set; }


    /// <summary>
    /// Atributo condicional para expresar el tipo de cambio conforme con la moneda registrada en el documento relacionado. Es requerido cuando la moneda del documento relacionado es distinta de la moneda de pago. Se debe registrar el número de unidades de la moneda señalada en el documento relacionado que equivalen a una unidad de la moneda del pago. Por ejemplo: El documento relacionado se registra en USD. El pago se realiza por 100 EUR. Este atributo se registra como 1.114700 USD/EUR. El importe pagado equivale a 100 EUR * 1.114700 USD/EUR = 111.47 USD.
    /// </summary>
    [XmlAttribute("EquivalenciaDR")]
    public decimal InvoiceExchangeRate { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el número de parcialidad que corresponde al pago.
    /// </summary>
    [XmlAttribute("NumParcialidad")]
    public int PartialityNumber { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el monto del saldo insoluto de la parcialidad anterior. En el caso de que sea la primer parcialidad este atributo debe contener el importe total del documento relacionado.
    /// </summary>
    [XmlAttribute("ImpSaldoAnt")]
    public decimal PreviousBalanceAmount { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el importe pagado para el documento relacionado.
    /// </summary>
    [XmlAttribute("ImpPagado")]
    public decimal PaymentAmount { get; set; }


    /// <summary>
    /// Atributo requerido para expresar la diferencia entre el importe del saldo anterior y el monto del pago.
    /// </summary>
    [XmlAttribute("ImpSaldoInsoluto")]
    public decimal RemainingBalance { get; set; }


    /// <summary>
    /// Atributo requerido para expresar si el pago del documento relacionado es objeto o no de impuesto.
    /// </summary>
    [XmlAttribute("ObjetoImpDR")]
    public string? TaxObjectId { get; set; }

    /// <summary>
    /// Nodo condicional para registrar los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda del documento relacionado.
    /// </summary>
    [XmlElement("ImpuestosDR")]
    public PaymentInvoiceTaxesWrapper? InvoiceTaxesWrapper { get; set; }


    #region DomainServices

    /// <summary>
    /// Add transferred tax to current payment invoice.
    /// </summary>
    /// <param name="paymentInvoiceTransferredTax">transferred tax object</param>
    public void AddTransferredTax(PaymentInvoiceTransferredTax paymentInvoiceTransferredTax)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();
        InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
    }

    /// <summary>
    /// Add transferred taxes list to current payment invoice.
    /// </summary>
    /// <param name="paymentInvoiceTransferredTaxes">transferred tax list object</param>
    public void AddTransferredTaxes(List<PaymentInvoiceTransferredTax> paymentInvoiceTransferredTaxes)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();
        InvoiceTaxesWrapper.TransferredTaxes.AddRange(paymentInvoiceTransferredTaxes);
    }

    /// <summary>
    /// Add transferred tax to current payment invoice.
    /// Manually calculated
    /// </summary>
    /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
    /// <param name="amount">Importe:Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota.</param>
    public void AddTransferredTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();

        var paymentInvoiceTransferredTax = new PaymentInvoiceTransferredTax
        {
            Base = pbase,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = amount
        };

        InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
    }

    /// <summary>
    /// Add transferred tax to current invoice item.
    /// Self-calculating
    /// </summary>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:|Tasa|Cuota|Exento|:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
    public void AddTransferredTax(string taxId, string taxTypeId, decimal taxRate = 0)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();

        var paymentInvoiceTransferredTax = new PaymentInvoiceTransferredTax()
        {
            Base = PaymentAmount,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = PaymentAmount * taxRate
        };

        InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
    }

    /// <summary>
    /// Add withholding tax to current payment invoice.
    /// </summary>
    /// <param name="paymentInvoiceWithholdingTax">withholding tax object</param>
    public void AddWithholdingTax(PaymentInvoiceWithholdingTax paymentInvoiceWithholdingTax)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();
        InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
    }

    /// <summary>
    /// Add withholding tax list to current payment invoice.
    /// </summary>
    /// <param name="paymentInvoiceWithholdingTaxes">withholding tax list object</param>
    public void AddWithholdingTaxes(List<PaymentInvoiceWithholdingTax> paymentInvoiceWithholdingTaxes)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();
        InvoiceTaxesWrapper.WithholdingTaxes.AddRange(paymentInvoiceWithholdingTaxes);
    }

    /// <summary>
    /// Add withholding tax to current payment invoice.
    /// Manually calculated
    /// </summary>
    /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo de la retención, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.</param>
    /// <param name="amount">Importe:Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. No se permiten valores negativos.</param>
    public void AddWithholdingTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();


        var paymentInvoiceWithholdingTax = new PaymentInvoiceWithholdingTax
        {
            Base = pbase,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = amount
        };

        InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
    }

    /// <summary>
    /// Add withholding tax to current payment invoice.
    /// Self-calculating
    /// </summary>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.</param>
    public void AddWithholdingTax(string taxId, string taxTypeId, decimal taxRate)
    {
        InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
        InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();

        var paymentInvoiceWithholdingTax = new PaymentInvoiceWithholdingTax()
        {
            Base = PaymentAmount,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = PaymentAmount * taxRate
        };

        InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
    }

    #endregion
}