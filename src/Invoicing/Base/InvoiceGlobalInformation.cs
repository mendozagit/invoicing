using System.Xml.Serialization;

namespace Invoicing.Base
{
    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    public sealed class InvoiceGlobalInformation
    {
        /// <summary>
        /// Atributo requerido para expresar el período al que corresponde la información del comprobante global.
        /// </summary>
        [XmlAttribute(AttributeName = "Periodicidad")]
        public string? Periodicity { get; set; }

        /// <summary>
        /// Atributo requerido para expresar el mes o los meses al que corresponde la información del comprobante global.
        /// </summary>
        [XmlAttribute(AttributeName = "Meses")]
        public string? Month { get; set; }

        /// <summary>
        /// Atributo requerido para expresar el año al que corresponde la información del comprobante global.
        /// </summary>
        [XmlAttribute(AttributeName = "Año")]
        public int Year { get; set; }
    }
}