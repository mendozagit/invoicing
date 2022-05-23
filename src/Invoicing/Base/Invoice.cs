using Invoicing.Common;

namespace Invoicing.Base;

public class Invoice
{
    public InvoiceType InvoiceType { get; set; }
    public string? InvoiceDate { get; set; }
    public string? InvoiceVersion { get; set; }
    public string? InvoiceSerie { get; set; }
    public string? InvoiceNuber { get; set; }

    public string? SignatureValue { get; set; }
    public string? PaymentForm { get; set; }
    public string? CertificateNumber { get; set; }
    public string? PaymentConditions { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public string? Currency { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal Total { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ExpeditionZipCode { get; set; }
}