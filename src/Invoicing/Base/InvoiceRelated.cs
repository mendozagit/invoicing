using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceRelated
{
    [XmlAttribute(AttributeName = "UUID")]
    public string? InvoiceUuid { get; set; }
}