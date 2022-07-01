using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoicing.Base;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;
using Invoicing.Complements.Payments;

namespace Invoicing.Servicies
{
    public class PaymentService : InvoiceService, IHasStandardFields
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
            SerializerHelper.ConfigureSettingsForPayment();


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
        public void AddPayment(string? paymentDate, string? paymentFormId, string? currencyId, decimal exchangeRate,
            decimal ammount, string? operationNumber, string? originBankTin, string? originBankAccountNumber,
            string? destinationBankTin, string? destinationAccountNumber, string? foreignBankName = null,
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
                PaymentDate = paymentDate,
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
    }
}