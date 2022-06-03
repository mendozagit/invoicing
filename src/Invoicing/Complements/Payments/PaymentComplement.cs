using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

/// <summary>
/// Complemento para el Comprobante Fiscal Digital por Internet (CFDI) para registrar información sobre la recepción de pagos. El emisor de este complemento para recepción de pagos debe ser quien las leyes le obligue a expedir comprobantes por los actos o actividades que realicen, por los ingresos que se perciban o por las retenciones de contribuciones que efectúen.
/// </summary>
public class PaymentComplement
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

    [XmlArray(ElementName = "Pagos")]
    [XmlArrayItem(ElementName = "Pago")]
    public List<Payment>? Payments { get; set; }
}