using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

public class Payment
{
    /// <summary>
    /// Atributo requerido para expresar la fecha y hora en la que el beneficiario recibe el pago. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.En caso de no contar con la hora se debe registrar 12:00:00.
    /// </summary>
    [XmlAttribute("FechaPago")]
    public string? PaymentDate { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave de la forma en que se realiza el pago.
    /// </summary>
    [XmlAttribute("FormaDePagoP")]
    public string? PaymentFormId { get; set; }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para realizar el pago conforme a la especificación ISO 4217. Cuando se usa moneda nacional se registra MXN. El atributo Pagos:Pago:Monto debe ser expresado en la moneda registrada en este atributo.
    /// </summary>
    [XmlAttribute("MonedaP")]
    public string? CurrencyId { get; set; }


    /// <summary>
    /// Atributo condicional para expresar el tipo de cambio de la moneda a la fecha en que se realizó el pago. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo MonedaP. Es requerido cuando el atributo MonedaP es diferente a MXN.
    /// </summary>
    [XmlAttribute("TipoCambioP")]
    [DefaultValue(0)]
    public decimal ExchangeRate { get; set; }


    /// <summary>
    /// Atributo requerido para expresar el importe del pago.
    /// </summary>
    [XmlAttribute("Monto")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Atributo condicional para expresar el número de cheque, número de autorización, número de referencia, clave de rastreo en caso de ser SPEI, línea de captura o algún número de referencia análogo que identifique la operación que ampara el pago efectuado.
    /// </summary>
    [XmlAttribute("NumOperacion")]
    public string? OperationNumber { get; set; }


    //Origin and destination

    /// <summary>
    /// Atributo condicional para expresar la clave RFC de la entidad emisora de la cuenta origen, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc., en caso de ser extranjero colocar XEXX010101000, considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("RfcEmisorCtaOrd")]
    public string? OriginBankTin { get; set; }


    /// <summary>
    /// Atributo condicional para incorporar el número de la cuenta con la que se realizó el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("CtaOrdenante")]
    public string? OriginBankAccountNumber { get; set; }

    /// <summary>
    /// Atributo condicional para expresar la clave RFC de la entidad operadora de la cuenta destino, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("RfcEmisorCtaBen")]
    public string? DestinationBankTin { get; set; }

    /// <summary>
    /// Atributo condicional para incorporar el número de cuenta en donde se recibió el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("CtaBeneficiario")]
    public string? DestinationAccountNumber { get; set; }

    /// <summary>
    /// Atributo condicional para expresar el nombre del banco ordenante, es requerido en caso de ser extranjero. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    [XmlAttribute("NomBancoOrdExt")]
    public string? ForeignBankName { get; set; }


    /// <summary>
    /// Atributo condicional para identificar la clave del tipo de cadena de pago que genera la entidad receptora del pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.
    /// </summary>
    /// <value>01-SPEI</value>
    [XmlAttribute("TipoCadPago")]
    public string? ElectronicPaymentSystemId { get; set; }

    /// <summary>
    /// Atributo condicional que sirve para incorporar el certificado que ampara al pago, como una cadena de texto en formato base 64. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("CertPago")]
    public string? Base64PaymetCertificate { get; set; }

    /// <summary>
    /// Atributo condicional para expresar la cadena original del comprobante de pago generado por la entidad emisora de la cuenta beneficiaria. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("CadPago")]
    public string? PaymentOriginalString { get; set; }


    /// <summary>
    /// Atributo condicional para integrar el sello digital que se asocie al pago. La entidad que emite el comprobante de pago, ingresa una cadena original y el sello digital en una sección de dicho comprobante, este sello digital es el que se debe registrar en este atributo. Debe ser expresado como una cadena de texto en formato base 64. Es requerido en caso de que el atributo TipoCadPago contenga información.
    /// </summary>
    [XmlAttribute("SelloPago")]
    public string? SignatureValue { get; set; }


    /// <summary>
    /// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
    /// </summary>

    [XmlElement("DoctoRelacionado")]
    public List<PaymentInvoice>? Invoices { get; set; }


    /// <summary>
    /// Nodo condicional para registrar el resumen de los impuestos aplicables conforme al monto del pago recibido, expresados a la moneda de pago.
    /// </summary>
    [XmlElement("ImpuestosP")]
    public PaymentTaxesWrapper? PaymentTaxexWrapper { get; set; }

    #region DomainServices

    /// <summary>
    /// Add an invoice (related document) to current payment.
    /// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
    /// </summary>
    /// <param name="invoice"></param>
    public void AddInvoice(PaymentInvoice invoice)
    {
        Invoices ??= new List<PaymentInvoice>();
        Invoices.Add(invoice);
    }

    /// <summary>
    /// Add an invoice (related document) to current payment.
    /// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
    /// </summary>
    /// <param name="invoiceUuid">IdDocumento:Atributo requerido para expresar el identificador del documento relacionado con el pago. Este dato puede ser un Folio Fiscal de la Factura Electrónica o bien el número de operación de un documento digital.</param>
    /// <param name="invoiceSeries">Serie:Atributo opcional para precisar la serie del comprobante para control interno del contribuyente, acepta una cadena de caracteres.</param>
    /// <param name="invoiceNumber">Folio:Atributo opcional para precisar el folio del comprobante para control interno del contribuyente, acepta una cadena de caracteres.</param>
    /// <param name="invoiceCurrencyId">MonedaDR:Atributo requerido para identificar la clave de la moneda utilizada en los importes del documento relacionado, cuando se usa moneda nacional o el documento relacionado no especifica la moneda se registra MXN. Los importes registrados en los atributos “ImpSaldoAnt”, “ImpPagado” e “ImpSaldoInsoluto” de éste nodo, deben corresponder a esta moneda. Conforme con la especificación ISO 4217.</param>
    /// <param name="invoiceExchangeRate">EquivalenciaDR:Atributo condicional para expresar el tipo de cambio conforme con la moneda registrada en el documento relacionado. Es requerido cuando la moneda del documento relacionado es distinta de la moneda de pago. Se debe registrar el número de unidades de la moneda señalada en el documento relacionado que equivalen a una unidad de la moneda del pago. Por ejemplo: El documento relacionado se registra en USD. El pago se realiza por 100 EUR. Este atributo se registra como 1.114700 USD/EUR. El importe pagado equivale a 100 EUR * 1.114700 USD/EUR = 111.47 USD.</param>
    /// <param name="partialityNumber">NumParcialidad:Atributo requerido para expresar el número de parcialidad que corresponde al pago.</param>
    /// <param name="previousBalanceAmount">ImpSaldoAnt:Atributo requerido para expresar el monto del saldo insoluto de la parcialidad anterior. En el caso de que sea la primer parcialidad este atributo debe contener el importe total del documento relacionado.</param>
    /// <param name="paymentAmount">ImpPagado:Atributo requerido para expresar el importe pagado para el documento relacionado.</param>
    /// <param name="remainingBalance">ImpSaldoInsoluto:Atributo requerido para expresar la diferencia entre el importe del saldo anterior y el monto del pago.</param>
    /// <param name="taxObjectId">ObjetoImpDR:Atributo requerido para expresar si el pago del documento relacionado es objeto o no de impuesto.</param>
    public void AddInvoice(
        string? invoiceUuid,
        string? invoiceSeries,
        string? invoiceNumber,
        string? invoiceCurrencyId,
        decimal invoiceExchangeRate,
        int partialityNumber,
        decimal previousBalanceAmount,
        decimal paymentAmount,
        decimal remainingBalance,
        string? taxObjectId)
    {
        Invoices ??= new List<PaymentInvoice>();


        var invoice = new PaymentInvoice
        {
            InvoiceUuid = invoiceUuid,
            InvoiceSeries = invoiceSeries,
            InvoiceNumber = invoiceNumber,
            InvoiceCurrencyId = invoiceCurrencyId,
            InvoiceExchangeRate = invoiceExchangeRate,
            PartialityNumber = partialityNumber,
            PreviousBalanceAmount = previousBalanceAmount,
            PaymentAmount = paymentAmount,
            RemainingBalance = remainingBalance,
            TaxObjectId = taxObjectId,
        };
        Invoices.Add(invoice);
    }


    /// <summary>
    /// Add transferred tax to current payment.
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// </summary>
    /// <param name="paymentTransferredTax">transferred tax object</param>
    public void AddTransferredTax(PaymentTransferredTax paymentTransferredTax)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentTransferredTaxes ??= new List<PaymentTransferredTax>();
        PaymentTaxexWrapper.PaymentTransferredTaxes.Add(paymentTransferredTax);
    }

    /// <summary>
    /// Add transferred tax to current payment.
    /// Manually calculated
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// </summary>
    /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
    /// <param name="amount">Importe:Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota.</param>
    public void AddTransferredTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentTransferredTaxes ??= new List<PaymentTransferredTax>();


        var paymentTransferredTax = new PaymentTransferredTax
        {
            Base = pbase,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = amount
        };

        PaymentTaxexWrapper.PaymentTransferredTaxes.Add(paymentTransferredTax);
    }

    /// <summary>
    /// Add transferred tax to current invoice item.
    /// Self-calculating
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// </summary>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
    /// <param name="taxTypeId">TipoFactor:|Tasa|Cuota|Exento|:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
    /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
    public void AddTransferredTax(string taxId, string taxTypeId, decimal taxRate = 0)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentTransferredTaxes ??= new List<PaymentTransferredTax>();

        var paymentTransferredTax = new PaymentTransferredTax()
        {
            Base = Amount,
            TaxId = taxId,
            TaxTypeId = taxTypeId,
            TaxRate = taxRate,
            Amount = Amount * taxRate
        };

        PaymentTaxexWrapper.PaymentTransferredTaxes.Add(paymentTransferredTax);
    }

    /// <summary>
    /// Add withholding tax to current payment.
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// </summary>
    /// <param name="paymentWithholdingTax">withholding tax object</param>
    public void AddWithholdingTax(PaymentWithholdingTax paymentWithholdingTax)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentWithholdingTaxes ??= new List<PaymentWithholdingTax>();
        PaymentTaxexWrapper.PaymentWithholdingTaxes.Add(paymentWithholdingTax);
    }

    /// <summary>
    /// Add withholding tax to current payment invoice.
    /// Manually calculated
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// </summary>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
    /// <param name="amount">Importe:Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. No se permiten valores negativos.</param>
    public void AddWithholdingTax(string taxId, decimal amount)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentWithholdingTaxes ??= new List<PaymentWithholdingTax>();


        var paymentWithholdingTax = new PaymentWithholdingTax
        {
            // Base = pbase,
            TaxId = taxId,
            //TaxTypeId = taxTypeId,
            //TaxRate = taxRate,
            Amount = amount
        };

        PaymentTaxexWrapper.PaymentWithholdingTaxes.Add(paymentWithholdingTax);
    }

    /// <summary>
    /// Add withholding tax to current payment.
    /// You can optionally use this method if you do NOT want to use the Compute method for automatic calculation.
    /// Self-calculating
    /// </summary>
    /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
    public void AddWithholdingTax(string taxId)
    {
        PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
        PaymentTaxexWrapper.PaymentWithholdingTaxes ??= new List<PaymentWithholdingTax>();


        var paymentInvoiceWithholdingTax = new PaymentWithholdingTax()
        {
            TaxId = taxId,
            Amount = Amount
        };

        PaymentTaxexWrapper.PaymentWithholdingTaxes.Add(paymentInvoiceWithholdingTax);
    }

    #endregion
}