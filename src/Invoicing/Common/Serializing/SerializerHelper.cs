using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Invoicing.Common.Constants;
using Invoicing.Common.Enums;

namespace Invoicing.Common.Serializing
{
    /// <summary>
    /// XML serializer helper class. Serializes and deserializes objects from/to XML
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize/deserialize.
    /// Must have a parameterless constructor and implement <see cref="Serializable"/></typeparam>
    public class SerializerHelper
    {
        public static string? SchemaLocation { get; set; }
        public static XmlSerializerNamespaces Namespaces { get; set; } = new();


        public static void ConfigureSettingsForInvoice(InvoiceVersion version = InvoiceVersion.V40)
        {
            #region Invoice settings example

            //< cfdi:Comprobante
            //xmlns:cfdi = "http://www.sat.gob.mx/cfd/4"
            //xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance"
            //xsi: schemaLocation = "
            //http://www.sat.gob.mx/cfd/4
            //http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd"
            //</ cfdi:Comprobante >

            #endregion

            switch (version)
            {
                case InvoiceVersion.V40:
                    Namespaces = new XmlSerializerNamespaces();
                    Namespaces.Add("cfdi", InvoiceConstants.SatInvoice40Namespace);
                    Namespaces.Add("xsi", InvoiceConstants.SatInvoiceXsiNamespace);
                    SchemaLocation = $"{InvoiceConstants.SatInvoice40Namespace} {InvoiceConstants.SatInvoice40Schema}";
                    break;
                case InvoiceVersion.V33:
                    // Expired by mexican tax authority (SAT)
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(version), version, null);
            }
        }

        public static void ConfigureSettingsForPayment(PaymentVersion version = PaymentVersion.V20)
        {
            #region Invoice settings example

            //< cfdi:Comprobante
            //xmlns: cfdi = "http://www.sat.gob.mx/cfd/4"
            //xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance"
            //xmlns: Pagos20 = "http://www.sat.gob.mx/Pagos20"
            //xsi: schemaLocation = "
            //http://www.sat.gob.mx/cfd/4
            //http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd
            //http://www.sat.gob.mx/Pagos20
            //http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd"
            //<cfdi:/Comprobante >

            #endregion

            switch (version)
            {
                case PaymentVersion.V20:
                    Namespaces = new XmlSerializerNamespaces();
                    Namespaces.Add("cfdi", InvoiceConstants.SatInvoice40Namespace);
                    Namespaces.Add("xsi", InvoiceConstants.SatInvoiceXsiNamespace);
                    Namespaces.Add("Pagos20", InvoiceConstants.SatPayment20Namespace);
                    SchemaLocation =
                        $"{InvoiceConstants.SatInvoice40Namespace} {InvoiceConstants.SatInvoice40Schema} {InvoiceConstants.SatPayment20Namespace} {InvoiceConstants.SatPayment20Schema}";
                    break;
                case PaymentVersion.V10:
                    // Expired by mexican tax authority (SAT)
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(version), version, null);
            }
        }
    }
}