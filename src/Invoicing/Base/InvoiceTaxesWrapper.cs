using System.ComponentModel;
using System.Xml.Serialization;

namespace Invoicing.Base
{
    public sealed class InvoiceTaxesWrapper
    {
        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos retenidos que se desprenden de los conceptos expresados en el comprobante fiscal digital por Internet. No se permiten valores negativos. Es requerido cuando en los conceptos se registren impuestos retenidos.
        /// </summary>
        [XmlAttribute("TotalImpuestosRetenidos")]
        [DefaultValue(0)]
        public decimal TotalWithholdingTaxes { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos trasladados que se desprenden de los conceptos expresados en el comprobante fiscal digital por Internet. No se permiten valores negativos. Es requerido cuando en los conceptos se registren impuestos trasladados.
        /// </summary>
        [XmlAttribute("TotalImpuestosTrasladados")]
        [DefaultValue(0)]
        public decimal TotalTransferredTaxes { get; set; }


        /// <summary>
        /// Nodo condicional para capturar los impuestos retenidos aplicables. Es requerido cuando en los conceptos se registre algún impuesto retenido.
        /// </summary>
        [XmlArray(ElementName = "Retenciones")]
        [XmlArrayItem(ElementName = "Retencion")]
        public List<InvoiceWithholdingTax>? WithholdingTaxes { get; set; }

        /// <summary>
        /// Nodo condicional para capturar los impuestos trasladados aplicables. Es requerido cuando en los conceptos se registre un impuesto trasladado.
        /// </summary>
        [XmlArray(ElementName = "Traslados")]
        [XmlArrayItem(ElementName = "Traslado")]
        public List<InvoiceTransferredTax>? TransferredTaxes { get; set; }
    }
}