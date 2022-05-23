using System.Xml.Serialization;
using Invoicing.Common;

namespace Invoicing.Base;

[XmlType(TypeName = "Emisor", Namespace = InvoiceConstants.NamespaceV40)]
public class InvoiceIssuer
{
    [XmlAttribute(AttributeName = "Rfc")] public string? Tin { get; set; }

    [XmlAttribute(AttributeName = "Nombre")]
    public string? LegalName { get; set; }

    [XmlAttribute(AttributeName = "RegimenFiscal")]
    public string? TaxRegime { get; set; }
}