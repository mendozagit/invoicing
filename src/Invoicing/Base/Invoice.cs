using System.Xml.Serialization;
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


    [XmlArray(ElementName = "CfdiRelacioandos", Namespace = InvoiceConstants.NamespaceV40)]
    [XmlArrayItem(ElementName = "CfdiRelacioando", Namespace = InvoiceConstants.NamespaceV40)]
    public List<InvoiceRelated>? InvoiceRelateds { get; set; }


    [XmlElement(ElementName = "Emisor")] public InvoiceIssuer? InvoiceIssuer { get; set; }


    [XmlElement(ElementName = "Receptor")] 
    public InvoiceRecipient? InvoiceRecipient { get; set; }


    [XmlArray(ElementName = "Conceptos", Namespace = InvoiceConstants.NamespaceV40)]
    [XmlArrayItem(ElementName = "Concepto", Namespace = InvoiceConstants.NamespaceV40)]
    public List<InvoiceItem>? InvoiceItems { get; set; } = new();


}