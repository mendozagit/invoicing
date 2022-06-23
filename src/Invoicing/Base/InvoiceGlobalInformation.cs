using System.Xml.Serialization;

namespace Invoicing.Base
{
    public class InvoiceGlobalInformation
    {
        [XmlAttribute(AttributeName = "Periodicidad")]
        public string? Periodicity { get; set; }

        [XmlAttribute(AttributeName = "Meses")]
        public string? Month { get; set; }

        [XmlAttribute(AttributeName = "Año")] 
        public int Year { get; set; }
    }
}