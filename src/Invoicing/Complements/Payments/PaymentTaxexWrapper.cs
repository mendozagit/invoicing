using System.Xml.Serialization;

namespace Invoicing.Complements.Payments;

public class PaymentTaxexWrapper
{
    [XmlArray("RetencionesP")]
    [XmlArrayItem("RetencionP")]
    public List<PaymentWithholdingTax>? WithholdingTaxes { get; set; }

    [XmlArray("TrasladosP")]
    [XmlArrayItem("TrasladoP")]
    public List<PaymentTransferredTax>? TransferredTaxes { get; set; }
}