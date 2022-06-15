using System.Xml.Serialization;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Complemento para el Comprobante Fiscal Digital por Internet (CFDI) para registrar información sobre la recepción de pagos. El emisor de este complemento para recepción de pagos debe ser quien las leyes le obligue a expedir comprobantes por los actos o actividades que realicen, por los ingresos que se perciban o por las retenciones de contribuciones que efectúen.
/// </summary>
[XmlRoot("Pagos", Namespace = InvoiceConstants.SatPayment20Namespace)]
public class PaymentComplement : ComputeSettings, IComputable
{
    /// <summary>
    /// Nodo requerido para especificar el monto total de los pagos y el total de los impuestos, deben ser expresados en MXN.
    /// </summary>
    [XmlElement("Totales")]
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
            //Rouding payment invoice values to header decimal places

            RoundRelatedPaymentInvoicesValues(payment);


            //Generate related invoice's transferred taxes
            var invoiceTransferredTaxes = ComputeInvoiceTranferredTaxes(payment);

            //Generate related invoice's witholding taxes
            var invoiceWitholdingTaxes = ComputeInvoiceWithholdingTaxes(payment);


            //Generate payment's transferred taxes from invoice's transferred taxes
            ComputePaymentTranferredTaxes(payment, invoiceTransferredTaxes);


            //Generate payment's witholding taxes from invoice's witholding taxes
            ComputePaymentWithholdingTaxes(payment, invoiceWitholdingTaxes);

            //Compute payment summary
            ComputePaymentSummary(payment, invoiceWitholdingTaxes, invoiceTransferredTaxes);

            RemoveUnnecessaryElements();
        }
    }

    private void RoundRelatedPaymentInvoicesValues(Payment payment)
    {
        if (payment.Invoices is null)
            throw new ArgumentNullException(nameof(payment.Invoices));

        foreach (var invoice in payment.Invoices)
        {
            invoice.PaymentAmount = invoice.PaymentAmount.ToSatRounding(HeaderDecimals, RoundingStrategy);
            invoice.InvoiceExchangeRate = invoice.InvoiceExchangeRate.ToSatRounding(HeaderDecimals, RoundingStrategy);
            invoice.PreviousBalanceAmount =
                invoice.PreviousBalanceAmount.ToSatRounding(HeaderDecimals, RoundingStrategy);
            invoice.RemainingBalance = invoice.RemainingBalance.ToSatRounding(HeaderDecimals, RoundingStrategy);
        }
    }

    private void RemoveUnnecessaryElements()
    {
    }

    private void ComputePaymentSummary(Payment payment, List<PaymentInvoiceWithholdingTax> witholdingTaxes,
        List<PaymentInvoiceTransferredTax> transferredTaxes)
    {
        var paymentSummary = new PaymentSummary();


        paymentSummary.WithholdingIva += witholdingTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()))
            .Select(x => x.Amount).Sum();


        paymentSummary.WithholdingIsr += witholdingTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Isr.ToValue()))
            .Select(x => x.Amount).Sum();


        paymentSummary.WithholdingIeps += witholdingTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Ieps.ToValue()))
            .Select(x => x.Amount).Sum();


        paymentSummary.TransferredIva16Base += transferredTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) && x.TaxRate == 0.160000m)
            .Select(x => x.Base).Sum();


        paymentSummary.TransferredIva16 = transferredTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) && x.TaxRate == 0.160000m)
            .Select(x => x.Amount).Sum();


        paymentSummary.TransferredIva8Base += transferredTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) && x.TaxRate == 0.080000m)
            .Select(x => x.Base).Sum();


        paymentSummary.TransferredIva8 += transferredTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) && x.TaxRate == 0.080000m)
            .Select(x => x.Amount).Sum();


        paymentSummary.TransferredIva0Base += transferredTaxes
            .Where(x => x.TaxTypeId != null && x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) &&
                        x.TaxRate == 0.000000m && !x.TaxTypeId.Equals(TaxType.Exento.ToValue()))
            .Select(x => x.Base).Sum();

        //The base field is used Amount instead of the base because the rate is zero but is not 'Exento', is VAT 0.
        paymentSummary.TransferredIva0 = transferredTaxes
            .Where(x => x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) && x.TaxRate == 0.000000m)
            .Select(x => x.Amount).Sum();


        //The base field is used instead of the amount because the rate is zero for both exempt and VAT0.
        //If the amount field is used, it will always generate a zero as the total, and the [DefaultValue(0)] attribute will omit this attribute during serialization.
        paymentSummary.TransferredIvaExcentoBase += transferredTaxes
            .Where(x =>
                x.TaxTypeId is not null &&
                x.TaxId is not null && x.TaxId.Equals(Tax.Iva.ToValue()) &&
                x.TaxTypeId.Equals(TaxType.Exento.ToValue()))
            .Select(x => x.Base).Sum();


        //Compute PaymentAmount from invoice's paymentAmount
        if (payment.Invoices is not null)
        {
            paymentSummary.TotalPaymentAmount += payment.Invoices.Select(invoice => invoice.PaymentAmount).Sum();
        }


        paymentSummary.WithholdingIva = paymentSummary.WithholdingIva.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.WithholdingIsr = paymentSummary.WithholdingIsr.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.WithholdingIeps = paymentSummary.WithholdingIeps.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva16Base =
            paymentSummary.TransferredIva16Base.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva16 =
            paymentSummary.TransferredIva16.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva8Base =
            paymentSummary.TransferredIva8Base.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva8 = paymentSummary.TransferredIva8.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva0Base =
            paymentSummary.TransferredIva0Base.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TransferredIva0 = paymentSummary.TransferredIva0.ToSatRounding(HeaderDecimals, RoundingStrategy);


        paymentSummary.TransferredIvaExcentoBase =
            paymentSummary.TransferredIvaExcentoBase.ToSatRounding(HeaderDecimals, RoundingStrategy);
        paymentSummary.TotalPaymentAmount =
            paymentSummary.TotalPaymentAmount.ToSatRounding(HeaderDecimals, RoundingStrategy);

        PaymentSummary = paymentSummary;
    }


    /// <summary>
    ///  //Generate related invoice's transferred taxes compliance SAT rules
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

                //Only 'Exento' taxes (transferred or withheld) must omit TaxRate(TasaOCuota) and Amount(Importe) attributes during serialization proccess
                if (paymentInvoiceTransferredTax.TaxTypeId != null &&
                    paymentInvoiceTransferredTax.TaxTypeId.Equals(TaxType.Exento.ToValue()))
                {
                    paymentInvoiceTransferredTax.TaxRateSpecified = false;
                    paymentInvoiceTransferredTax.AmountSpecified = false;
                }

                paymentInvoiceTransferredTaxes.Add(paymentInvoiceTransferredTax);
            }
        }


        return paymentInvoiceTransferredTaxes;
    }


    /// <summary>
    /// Generate related invoice's witholding taxes compliance SAT rules
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

                //Only 'Exento' taxes (transferred or withheld) must omit TaxRate(TasaOCuota) and Amount(Importe) attributes during serialization proccess
                if (paymentInvoiceWithholdingTax.TaxTypeId != null &&
                    paymentInvoiceWithholdingTax.TaxTypeId.Equals(TaxType.Exento.ToValue()))
                {
                    paymentInvoiceWithholdingTax.TaxRateSpecified = false;
                    paymentInvoiceWithholdingTax.AmountSpecified = false;
                }

                paymentInvoiceWithholdingTaxes.Add(paymentInvoiceWithholdingTax);
            }
        }


        return paymentInvoiceWithholdingTaxes;
    }

    /// <summary>
    /// Generate payment's transferred taxes from invoice's transferred taxes compliance SAT rules
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
            //Only 'Exento' taxes (transferred or withheld) must omit TaxRate(TasaOCuota) and Amount(Importe) attributes during serialization proccess
            if (paymentTransferredTax.TaxTypeId != null &&
                paymentTransferredTax.TaxTypeId.Equals(TaxType.Exento.ToValue()))
            {
                paymentTransferredTax.TaxRateSpecified = false;
                paymentTransferredTax.AmountSpecified = false;
            }


            payment.PaymentTaxexWrapper.PaymentTransferredTaxes.Add(paymentTransferredTax);
        }
    }


    /// <summary>
    /// Generate payment's witholding taxes from invoice's witholding taxes compliance SAT rules
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