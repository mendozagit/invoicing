using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceIssuer
{
    [XmlAttribute(AttributeName = "Rfc")]
    public string? Tin { get; set; }

    [XmlAttribute(AttributeName = "Nombre")]
    public string? LegalName { get; set; }

    [XmlAttribute(AttributeName = "RegimenFiscal")]
    public string? TaxRegime { get; set; }
}