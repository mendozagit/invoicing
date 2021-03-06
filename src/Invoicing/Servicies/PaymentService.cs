using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Credencials.Common;
using Invoicing.Base;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Exceptions;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;
using Invoicing.Complements.Payments;

namespace Invoicing.Servicies
{
    public class PaymentService : InvoiceService, IHasComplement
    {
        private PaymentComplement? _paymentComplement;

        public PaymentService()
        {
            ConfigureStandardFields();
        }

        /// <summary>
        /// Configures the invoice's standard fields, based on the complement their incorporated (complement to be added to invoice). 
        /// When a complement is incorporated into an invoice, it is very common that standard concepts are added to the invoice and
        /// several fields are omitted from the invoice header. This is a strategy to audit almost anything through the concept of
        /// "invoice complement", which is basically an invoice with a concept that has certain configuration such as identifier or
        /// special descriptions, as well as set some invoice fields to null as part of complement configuration. 
        /// </summary>
        public void ConfigureStandardFields()
        {
            // payment invoice item
            var standardInvoiceItem = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatPaymentItemId,
                //ItemId = "1801", Este campo no debe existir.
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatPaymentUnitOfMeasureId,
                //UnitOfMeasure = "PZA", Este campo no debe existir.
                Description = InvoiceConstants.SatPaymentItemDescriptionId,
                UnitCost = 0,
                Amount = 0,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatPaymentObjectId,
            };

            _invoice.InvoiceItems.Add(standardInvoiceItem);

            // Invoice header
            _invoice.InvoiceVersion = InvoiceVersion.V40;
            _invoice.InvoiceDate = DateTime.Now.ToSatFormat();
            _invoice.InvoiceTypeId = InvoiceType.Pago;
            _invoice.ExportId = "01"; //No aplica
        }


        /// <summary>
        /// Add a payment: Initializes the add-in when necessary, initializes the payment list, when necessary, and adds the payment to the list. 
        /// </summary>
        public void AddPayment(Payment payment)
        {
            //Create Payment complement and adds payment object
            _paymentComplement ??= new PaymentComplement
            {
                HeaderDecimals = _invoice.HeaderDecimals,
                ItemsDecimals = _invoice.ItemsDecimals,
                RoundingStrategy = _invoice.RoundingStrategy,
                PaymentSummary = new PaymentSummary(),
                Version = PaymentVersion.V20.ToValue(),
                Payments = null
            };
            _paymentComplement.Payments ??= new List<Payment>();
            _paymentComplement.Payments.Add(payment);
        }

        /// <summary>
        ///  Add payment list to invoice: Initializes the add-in when necessary, initializes the payment list, when necessary, and adds the payment to the list. 
        /// </summary>
        /// <param name="payments"></param>
        public void AddPayments(List<Payment> payments)
        {
            //Create Payment complement and adds payment object
            _paymentComplement ??= new PaymentComplement
            {
                HeaderDecimals = _invoice.HeaderDecimals,
                ItemsDecimals = _invoice.ItemsDecimals,
                RoundingStrategy = _invoice.RoundingStrategy,
                PaymentSummary = new PaymentSummary(),
                Version = PaymentVersion.V20.ToValue(),
                Payments = null
            };
            _paymentComplement.Payments ??= new List<Payment>();
            _paymentComplement.Payments.AddRange(payments);
        }


        /// <summary>
        ///Add a payment: Initializes the add-in when necessary, initializes the payment list, when necessary, and adds the payment to the list.  
        /// </summary>
        /// <param name="paymentDate">FechaPago:Atributo requerido para expresar la fecha y hora en la que el beneficiario recibe el pago. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.En caso de no contar con la hora se debe registrar 12:00:00.</param>
        /// <param name="paymentFormId">FormaDePagoP:Atributo requerido para expresar la clave de la forma en que se realiza el pago.</param>
        /// <param name="currencyId">MonedaP:Atributo requerido para identificar la clave de la moneda utilizada para realizar el pago conforme a la especificación ISO 4217. Cuando se usa moneda nacional se registra MXN. El atributo Pagos:Pago:Monto debe ser expresado en la moneda registrada en este atributo.</param>
        /// <param name="exchangeRate">TipoCambioP:Atributo condicional para expresar el tipo de cambio de la moneda a la fecha en que se realizó el pago. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo MonedaP. Es requerido cuando el atributo MonedaP es diferente a MXN.</param>
        /// <param name="ammount">Monto:Atributo requerido para expresar el importe del pago.</param>
        /// <param name="operationNumber">NumOperacion:Atributo condicional para expresar el número de cheque, número de autorización, número de referencia, clave de rastreo en caso de ser SPEI, línea de captura o algún número de referencia análogo que identifique la operación que ampara el pago efectuado</param>
        /// <param name="originBankTin">RfcEmisorCtaOrd:de la entidad emisora de la cuenta origen, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc., en caso de ser extranjero colocar XEXX010101000, considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.</param>
        /// <param name="originBankAccountNumber">CtaOrdenante:Atributo condicional para incorporar el número de la cuenta con la que se realizó el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.</param>
        /// <param name="destinationBankTin">RfcEmisorCtaBen:Atributo condicional para expresar la clave RFC de la entidad operadora de la cuenta destino, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.</param>
        /// <param name="destinationAccountNumber">CtaBeneficiario:Atributo condicional para incorporar el número de cuenta en donde se recibió el pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago</param>
        /// <param name="foreignBankName">NomBancoOrdExt:Atributo condicional para expresar el nombre del banco ordenante, es requerido en caso de ser extranjero. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.</param>
        /// <param name="electronicPaymentSystemId">TipoCadPago:Atributo condicional para identificar la clave del tipo de cadena de pago que genera la entidad receptora del pago. Considerar las reglas de obligatoriedad publicadas en la página del SAT para éste atributo de acuerdo con el catálogo catCFDI:c_FormaPago.</param>
        /// <param name="base64PaymetCertificate">CertPago:Atributo condicional que sirve para incorporar el certificado que ampara al pago, como una cadena de texto en formato base 64. Es requerido en caso de que el atributo TipoCadPago contenga información.</param>
        /// <param name="paymentOriginalString">CadPago:Atributo condicional para expresar la cadena original del comprobante de pago generado por la entidad emisora de la cuenta beneficiaria. Es requerido en caso de que el atributo TipoCadPago contenga información.</param>
        /// <param name="signatureValue">SelloPago:Atributo condicional para integrar el sello digital que se asocie al pago. La entidad que emite el comprobante de pago, ingresa una cadena original y el sello digital en una sección de dicho comprobante, este sello digital es el que se debe registrar en este atributo. Debe ser expresado como una cadena de texto en formato base 64. Es requerido en caso de que</param>
        public void AddPayment(DateTime paymentDate, string? paymentFormId, decimal ammount, string? currencyId = "MXN",
            decimal exchangeRate = 1, string? operationNumber = null, string? originBankTin = null,
            string? originBankAccountNumber = null,
            string? destinationBankTin = null, string? destinationAccountNumber = null, string? foreignBankName = null,
            string? electronicPaymentSystemId = null, string? base64PaymetCertificate = null,
            string? paymentOriginalString = null,
            string? signatureValue = null)
        {
            //Create Payment complement and adds payment object
            _paymentComplement ??= new PaymentComplement
            {
                HeaderDecimals = _invoice.HeaderDecimals,
                ItemsDecimals = _invoice.ItemsDecimals,
                RoundingStrategy = _invoice.RoundingStrategy,
                PaymentSummary = new PaymentSummary(),
                Version = PaymentVersion.V20.ToValue(),
                Payments = null
            };
            _paymentComplement.Payments ??= new List<Payment>();


            var payment = new Payment
            {
                PaymentDate = paymentDate.ToSatFormat(),
                PaymentFormId = paymentFormId,
                CurrencyId = currencyId,
                ExchangeRate = exchangeRate,
                Amount = ammount,
                OperationNumber = operationNumber,
                OriginBankTin = originBankTin,
                OriginBankAccountNumber = originBankAccountNumber,
                DestinationBankTin = destinationBankTin,
                DestinationAccountNumber = destinationAccountNumber,
                ForeignBankName = foreignBankName,
                ElectronicPaymentSystemId = electronicPaymentSystemId,
                Base64PaymetCertificate = base64PaymetCertificate,
                PaymentOriginalString = paymentOriginalString,
                SignatureValue = signatureValue,
            };


            _paymentComplement.Payments.Add(payment);
        }


        /// <inheritdoc />
        public override string SerializeToString()
        {
            SerializerHelper.ConfigureSettingsForPayment();

            _invoice.SchemaLocation = SerializerHelper.SchemaLocation;
            var settings = new XmlWriterSettings
            {
                CloseOutput = true,
                Encoding = Encoding.UTF8,
                Indent = false
            };
            var xml = Serializer<Invoice>.Serialize(_invoice, SerializerHelper.Namespaces, settings);
            return xml.Clean();
        }

        /// <inheritdoc />
        public override void SerializeToFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath),
                    "The path to write the payment-invoice.xml must not be null");


            SerializerHelper.ConfigureSettingsForInvoice();

            var settings = new XmlWriterSettings
            {
                CloseOutput = true,
                Encoding = Encoding.UTF8,
                Indent = true
            };
            Serializer<Invoice>.SerializeToFile(_invoice, filePath, SerializerHelper.Namespaces, settings);
        }

        /// <inheritdoc />
        public void AddInvoiceComplement(bool compute = true)
        {
            if (_paymentComplement is null)
                throw new InvoiceComplementNotFoundException("_paymentComplement is null");


            if (compute)
                _paymentComplement.Compute();

            //insert complement into invoice
            var paymentComplementXml =
                Serializer<Invoice>.SerializeElement(_paymentComplement,
                    InvoiceConstants.SatPayment20Namespace.GetSerializerNamespace("pago20"));
            _invoice.AddComplement(paymentComplementXml);
        }

        /// <inheritdoc />
        public override string SignInvoice(bool compute = true)
        {
            if (Credential is null)
                throw new CredentialNotFoundException(
                    "The credential object has not been set in payment invoice service.");


            if (string.IsNullOrEmpty(CredentialSettings.OriginalStringPath))
                throw new CredentialConfigurationException(
                    "The path to the xslt schemas was not set in CredentialSettings.");

            //Compute and insert complement into invoice
            AddInvoiceComplement();

            //Compute invoice and calculate OriginalString
            var originalStr = ComputeOriginalString(compute);
            var signature = Credential.SignData(originalStr);
            _invoice.SignatureValue = signature.ToBase64String();

            return originalStr;
        }


        #region Domain methods

        /// <summary>
        /// Add an invoice (related document) to last payment added.
        /// Nodo requerido para expresar la lista de documentos relacionados con los pagos. Por cada documento que se relacione se debe generar un nodo DoctoRelacionado.
        /// </summary>
        /// <param name="invoice"></param>
        public void AddPaymentInvoice(PaymentInvoice invoice)
        {
            _paymentComplement ??= new PaymentComplement();
            _paymentComplement.Payments ??= new List<Payment>();
            var payment = _paymentComplement.Payments.LastOrDefault();

            if (payment is null)
                throw new LastPaymentNotFoundException("No has payments to add paymentinvoice object.");

            payment.Invoices ??= new List<PaymentInvoice>();
            payment.Invoices.Add(invoice);
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
        public void AddPaymentInvoice(
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
            _paymentComplement ??= new PaymentComplement();
            _paymentComplement.Payments ??= new List<Payment>();
            var payment = _paymentComplement.Payments.LastOrDefault();

            if (payment is null)
                throw new LastPaymentNotFoundException("No has payments to add paymentinvoice object.");

            payment.Invoices ??= new List<PaymentInvoice>();


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
            payment.Invoices.Add(invoice);
        }


        /// <summary>
        /// Add transferred tax to last PaymentInvoice added.
        /// </summary>
        /// <param name="paymentInvoiceTransferredTax">transferred tax object</param>
        public void AddTransferredTax(PaymentInvoiceTransferredTax paymentInvoiceTransferredTax)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();
            paymentInvoiceTransferredTax.TaxRate = paymentInvoiceTransferredTax.TaxRate.ToSatRounding();
            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
        }

        /// <summary>
        /// Add transferred taxes list to last PaymentInvoice added.
        /// </summary>
        /// <param name="paymentInvoiceTransferredTaxes">transferred tax list object</param>
        public void AddTransferredTaxes(List<PaymentInvoiceTransferredTax>? paymentInvoiceTransferredTaxes)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            if (paymentInvoiceTransferredTaxes is null)
                throw new ArgumentNullException(nameof(paymentInvoiceTransferredTaxes));


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();

            foreach (var paymentInvoiceTransferredTax in paymentInvoiceTransferredTaxes)
                paymentInvoiceTransferredTax.TaxRate = paymentInvoiceTransferredTax.TaxRate.ToSatRounding();


            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes.AddRange(paymentInvoiceTransferredTaxes);
        }

        /// <summary>
        /// Add transferred tax to last PaymentInvoice added.
        /// Manually calculated
        /// </summary>
        /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
        /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
        /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
        /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
        /// <param name="amount">Importe:Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota.</param>
        public void AddTransferredTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();

            var paymentInvoiceTransferredTax = new PaymentInvoiceTransferredTax
            {
                Base = pbase,
                TaxId = taxId,
                TaxTypeId = taxTypeId,
                TaxRate = taxRate.ToSatRounding(),
                Amount = amount
            };

            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
        }

        /// <summary>
        /// Add transferred tax to last PaymentInvoice added.
        /// Self-calculating
        /// </summary>
        /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.</param>
        /// <param name="taxTypeId">TipoFactor:|Tasa|Cuota|Exento|:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
        /// <param name="taxRate">TasaOCuota:Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.</param>
        public void AddTransferredTax(string taxId, string taxTypeId, decimal taxRate = 0)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes ??= new List<PaymentInvoiceTransferredTax>();


            var paymentInvoiceTransferredTax = new PaymentInvoiceTransferredTax()
            {
                Base = lastPaymentInvoice.PaymentAmount,
                TaxId = taxId,
                TaxTypeId = taxTypeId,
                TaxRate = taxRate.ToSatRounding(),
                Amount = lastPaymentInvoice.PaymentAmount * taxRate.ToSatRounding()
            };

            lastPaymentInvoice.InvoiceTaxesWrapper.TransferredTaxes.Add(paymentInvoiceTransferredTax);
        }

        /// <summary>
        /// Add withholding tax to last PaymentInvoice added.
        /// </summary>
        /// <param name="paymentInvoiceWithholdingTax">withholding tax object</param>
        public void AddWithholdingTax(PaymentInvoiceWithholdingTax paymentInvoiceWithholdingTax)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();

            paymentInvoiceWithholdingTax.TaxRate = paymentInvoiceWithholdingTax.TaxRate.ToSatRounding();
            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
        }

        /// <summary>
        /// Add withholding tax list to last PaymentInvoice added.
        /// </summary>
        /// <param name="paymentInvoiceWithholdingTaxes">withholding tax list object</param>
        public void AddWithholdingTaxes(List<PaymentInvoiceWithholdingTax>? paymentInvoiceWithholdingTaxes)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            if (paymentInvoiceWithholdingTaxes is null)
                throw new ArgumentNullException(nameof(paymentInvoiceWithholdingTaxes));

            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();


            foreach (var paymentInvoiceWithholdingTax in paymentInvoiceWithholdingTaxes)
                paymentInvoiceWithholdingTax.TaxRate = paymentInvoiceWithholdingTax.TaxRate.ToSatRounding();


            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes.AddRange(paymentInvoiceWithholdingTaxes);
        }

        /// <summary>
        /// Add withholding tax to last PaymentInvoice added.
        /// Manually calculated
        /// </summary>
        /// <param name="pbase">Base:Atributo requerido para señalar la base para el cálculo de la retención, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.</param>
        /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
        /// <param name="taxTypeId">TipoFactor:Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
        /// <param name="taxRate">TasaOCuota:Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.</param>
        /// <param name="amount">Importe:Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. No se permiten valores negativos.</param>
        public void AddWithholdingTax(decimal pbase, string taxId, string taxTypeId, decimal taxRate, decimal amount)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();


            var paymentInvoiceWithholdingTax = new PaymentInvoiceWithholdingTax
            {
                Base = pbase,
                TaxId = taxId,
                TaxTypeId = taxTypeId,
                TaxRate = taxRate.ToSatRounding(),
                Amount = amount
            };

            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
        }

        /// <summary>
        /// Add withholding tax to last PaymentInvoice added.
        /// Self-calculating
        /// </summary>
        /// <param name="taxId">Impuesto:Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.</param>
        /// <param name="taxTypeId">TipoFactor:|Tasa|Cuota|Exento|: Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.</param>
        /// <param name="taxRate">TasaOCuota:Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.</param>
        public void AddWithholdingTax(string taxId, string taxTypeId, decimal taxRate)
        {
            var lastPayment = _paymentComplement?.Payments?.LastOrDefault();

            if (lastPayment is null)
                throw new LastPaymentNotFoundException("Not found last Payment object.");


            var lastPaymentInvoice = lastPayment?.Invoices?.LastOrDefault();


            if (lastPaymentInvoice is null)
                throw new LastPaymentInvoiceNotFoundException("Not found last InvoicePayment object.");


            lastPaymentInvoice.InvoiceTaxesWrapper ??= new PaymentInvoiceTaxesWrapper();
            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes ??= new List<PaymentInvoiceWithholdingTax>();

            var paymentInvoiceWithholdingTax = new PaymentInvoiceWithholdingTax()
            {
                Base = lastPaymentInvoice.PaymentAmount,
                TaxId = taxId,
                TaxTypeId = taxTypeId,
                TaxRate = taxRate.ToSatRounding(),
                Amount = lastPaymentInvoice.PaymentAmount * taxRate.ToSatRounding()
            };

            lastPaymentInvoice.InvoiceTaxesWrapper.WithholdingTaxes.Add(paymentInvoiceWithholdingTax);
        }

        #endregion
    }
}