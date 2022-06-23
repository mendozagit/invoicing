using System.Xml.Serialization;

namespace Invoicing.Base;

public class InvoiceIssuer
{
    /// <summary>
    /// Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante.
    /// </summary>
    [XmlAttribute(AttributeName = "Rfc")]
    public string? Tin { get; set; }


    /// <summary>
    /// Atributo requerido para registrar el nombre, denominación o razón social del contribuyente inscrito en el RFC, del emisor del comprobante.
    /// </summary>
    [XmlAttribute(AttributeName = "Nombre")]
    public string? LegalName { get; set; }


    /// <summary>
    /// Atributo requerido para incorporar la clave del régimen del contribuyente emisor al que aplicará el efecto fiscal de este comprobante.
    /// </summary>
    [XmlAttribute(AttributeName = "RegimenFiscal")]
    public string? TaxRegimeId { get; set; }


    /// <summary>
    /// Atributo condicional para expresar el número de operación proporcionado por el SAT cuando se trate de un comprobante a través de un PCECFDI o un PCGCFDISP.
    /// </summary>
    [XmlAttribute(AttributeName = "FacAtrAdquirente")]
    public string? OperationNumber { get; set; }
}