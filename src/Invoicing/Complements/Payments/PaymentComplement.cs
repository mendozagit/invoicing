using System.Xml.Serialization;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Extensions;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Complemento para el Comprobante Fiscal Digital por Internet (CFDI) para registrar información sobre la recepción de pagos. El emisor de este complemento para recepción de pagos debe ser quien las leyes le obligue a expedir comprobantes por los actos o actividades que realicen, por los ingresos que se perciban o por las retenciones de contribuciones que efectúen.
/// </summary>
[XmlRoot("Pagos", Namespace = InvoiceConstants.CurrentInvoiceNamespace)]
public class PaymentComplement : ComputeSettings, IComputable
{
    public PaymentSummary? PaymentSummary { get; set; }


    /// <summary>
    /// Atributo requerido que indica la versión del complemento para recepción de pagos.
    /// </summary>
    [XmlAttribute("Version")]
    public string? Version { get; set; }

    /// <summary>
    /// Elemento requerido para incorporar la información de la recepción de pagos.
    /// </summary>

    [XmlElement("Pago")]
    public List<Payment>? Payments { get; set; }


    #region Helpers

    public void Compute()
    {
        if (Payments is null) return;

        foreach (var payment in Payments)
        {
            //Generate related invoice transferred taxes
            var invoiceTransferredTaxes = ComputeInvoiceTranferredTaxes(payment);

            //Generate related invoice witholding taxes
            var invoiceWitholdingTaxes = ComputeInvoiceWithholdingTaxes(payment);


            //Generate payment transferred taxes from invoice transferred taxes
            ComputePaymentTranferredTaxes(payment, invoiceTransferredTaxes);


            //Generate payment witholding taxes from invoice witholding taxes
            ComputePaymentWithholdingTaxes(payment, invoiceWitholdingTaxes);
        }
    }


    /// <summary>
    /// Generate related invoice transferred taxes compliance SAT rules
    /// </summary>
    /// <param name="payment">payment object</param>
    /// <returns>List of PaymentInvoiceTransferredTax rounded as Mexican TA rules (SAT) </returns>
    private List<PaymentInvoiceTransferredTax> ComputeInvoiceTranferredTaxes(Payment payment)
    {
        var paymentInvoiceTransferredTaxes = new List<PaymentInvoiceTransferredTax>();


        if (payment?.Invoices is null) return paymentInvoiceTransferredTaxes;


        foreach (var paymentInvoice in payment.Invoices)
        {
            if (paymentInvoice?.InvoiceTaxesWrapper?.InvoiceTransferredTaxes is null) continue;

            foreach (var paymentInvoiceTransferredTax in paymentInvoice?.InvoiceTaxesWrapper
                         ?.InvoiceTransferredTaxes!)
            {
                paymentInvoiceTransferredTax.Base = payment.Ammount.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceTransferredTax.TaxRate =
                    paymentInvoiceTransferredTax.TaxRate.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceTransferredTax.Amount =
                    paymentInvoiceTransferredTax.Base * paymentInvoiceTransferredTax.TaxRate;

                paymentInvoiceTransferredTax.Amount =
                    paymentInvoiceTransferredTax.Amount.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceTransferredTaxes.Add(paymentInvoiceTransferredTax);
            }
        }


        return paymentInvoiceTransferredTaxes;
    }


    /// <summary>
    /// Generate related invoice witholding taxes compliance SAT rules
    /// </summary>
    /// <param name="payment">payment object</param>
    /// <returns>List of PaymentInvoiceWithholdingTax rounded as Mexican TA rules (SAT) </returns>
    private List<PaymentInvoiceWithholdingTax> ComputeInvoiceWithholdingTaxes(Payment payment)
    {
        var paymentInvoiceWithholdingTaxes = new List<PaymentInvoiceWithholdingTax>();


        if (payment?.Invoices is null)
            return paymentInvoiceWithholdingTaxes;

        foreach (var paymentInvoice in payment.Invoices)
        {
            if (paymentInvoice?.InvoiceTaxesWrapper?.WithholdingTaxes is null) continue;

            foreach (var paymentInvoiceWithholdingTax in paymentInvoice?.InvoiceTaxesWrapper?.WithholdingTaxes!)
            {
                paymentInvoiceWithholdingTax.Base = payment.Ammount.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceWithholdingTax.TaxRate =
                    paymentInvoiceWithholdingTax.TaxRate.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceWithholdingTax.Amount =
                    paymentInvoiceWithholdingTax.Base * paymentInvoiceWithholdingTax.TaxRate;

                paymentInvoiceWithholdingTax.Amount =
                    paymentInvoiceWithholdingTax.Amount.ToSatRounding(ItemsDecimals, RoundingStrategy);

                paymentInvoiceWithholdingTaxes.Add(paymentInvoiceWithholdingTax);
            }
        }


        return paymentInvoiceWithholdingTaxes;
    }

    /// <summary>
    /// Generate the tranferred taxes from the payment from the invoice's tranferred taxes
    /// </summary>
    /// <param name="payment">payment object</param>
    /// <param name="invoiceTransferredTaxes">List of invoice's transferred taxes </param>
    private void ComputePaymentTranferredTaxes(Payment payment,
        List<PaymentInvoiceTransferredTax> invoiceTransferredTaxes)
    {
        //Generate transferred taxes from the payment from the transferred taxes on the invoice
        foreach (var invoiceTransferredTax in invoiceTransferredTaxes)
        {
            payment.PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
            payment.PaymentTaxexWrapper.PaymentTransferredTaxes ??= new List<PaymentTransferredTax>();

            var paymentTransferredTax = new PaymentTransferredTax()
            {
                Base = invoiceTransferredTax.Base,
                TaxId = invoiceTransferredTax.TaxId,
                TaxTypeId = invoiceTransferredTax.TaxTypeId,
                TaxRate = invoiceTransferredTax.TaxRate,
                Amount = invoiceTransferredTax.Amount
            };
            payment.PaymentTaxexWrapper.PaymentTransferredTaxes.Add(paymentTransferredTax);
        }
    }


    /// <summary>
    /// Generate the withholding taxes from the payment from the invoice's withholding taxes
    /// </summary>
    /// <param name="payment"></param>
    /// <param name="invoiceWithholdingTaxes">List of invoice's witholding taxes</param>
    private void ComputePaymentWithholdingTaxes(Payment payment,
        List<PaymentInvoiceWithholdingTax> invoiceWithholdingTaxes)
    {
        foreach (var invoiceWithholdingTax in invoiceWithholdingTaxes)
        {
            payment.PaymentTaxexWrapper ??= new PaymentTaxesWrapper();
            payment.PaymentTaxexWrapper.PaymentWithholdingTaxes ??= new List<PaymentWithholdingTax>();

            var paymentWithholdingTax = new PaymentWithholdingTax
            {
                TaxId = invoiceWithholdingTax.TaxId,
                Amount = invoiceWithholdingTax.Amount
            };
            payment.PaymentTaxexWrapper.PaymentWithholdingTaxes.Add(paymentWithholdingTax);
        }
    }

    #endregion
}