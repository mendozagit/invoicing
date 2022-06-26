using System.Xml.Serialization;

namespace Invoicing.Base;

/// <summary>
/// Nodo opcional para precisar la información de los comprobantes relacionados. (nota de credito)
/// </summary>
public sealed class InvoiceRelatedWrapper
{
    /// <summary>
    /// Nodo requerido para precisar la información de los comprobantes relacionados.
    /// </summary>
    [XmlElement("CfdiRelacionado")]
    public List<InvoiceRelated>? RelatedInvoices { get; set; }

    /// <summary>
    /// Atributo requerido para indicar la clave de la relación que existe entre éste que se está generando y el o los CFDI previos.
    /// </summary>
    [XmlAttribute("TipoRelacion")]
    public string? RelationshipTypeId { get; set; }
}