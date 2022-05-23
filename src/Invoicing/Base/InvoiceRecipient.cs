using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceRecipient
{
    [XmlAttribute(AttributeName = "Rfc")] 
    public string? Tin { get; set; }

    [XmlAttribute(AttributeName = "Nombre")]
    public string? LegalName { get; set; }

    [XmlAttribute(AttributeName = "UsoCFDI")]
    public string? InvoiceUse { get; set; }
}